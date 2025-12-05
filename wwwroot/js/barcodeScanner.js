// barcodeScanner helper - tries BarcodeDetector, falls back to html5-qrcode
(function () {
    window.barcodeScanner = {
        _html5Qrcode: null,
        _elementId: null,
        _stream: null,
        _interval: null,

        async start(dotNetRef, elementId) {
            this._elementId = elementId ?? 'barcode-scanner-container';

            // create container
            let container = document.getElementById(this._elementId);
            if (!container) {
                container = document.createElement('div');
                container.id = this._elementId;
                container.style.position = 'fixed';
                container.style.left = '0';
                container.style.top = '0';
                container.style.width = '100%';
                container.style.height = '100%';
                container.style.zIndex = '2147483647';
                container.style.background = 'rgba(0,0,0,0.75)';
                container.style.display = 'flex';
                container.style.alignItems = 'center';
                container.style.justifyContent = 'center';
                document.body.appendChild(container);
            }

            // inner UI
            container.innerHTML = '';
            const inner = document.createElement('div');
            inner.style.width = '100%';
            inner.style.maxWidth = '480px';
            inner.style.padding = '12px';
            inner.style.boxSizing = 'border-box';
            inner.style.background = '#000';
            inner.style.borderRadius = '8px';
            inner.style.textAlign = 'center';

            const video = document.createElement('video');
            video.style.width = '100%';
            video.style.height = 'auto';
            video.setAttribute('playsinline', '');

            const closeBtn = document.createElement('button');
            closeBtn.textContent = 'Fechar';
            closeBtn.style.marginTop = '8px';
            closeBtn.className = 'btn btn-secondary';
            closeBtn.onclick = () => { this.stop(); };

            inner.appendChild(video);
            inner.appendChild(closeBtn);
            container.appendChild(inner);

            // Try native BarcodeDetector first
            const supportsBarcodeDetector = ('BarcodeDetector' in window);
            if (supportsBarcodeDetector) {
                try {
                    // prefer environment camera
                    const constraints = { video: { facingMode: { ideal: 'environment' } } };
                    this._stream = await navigator.mediaDevices.getUserMedia(constraints);
                    video.srcObject = this._stream;
                    await video.play();

                    const formats = ['ean_13', 'ean_8', 'code_128', 'qr_code', 'upc_e', 'upc_a'];
                    let detector = null;
                    try { detector = new BarcodeDetector({ formats }); } catch (e) { detector = new BarcodeDetector(); }

                    const canvas = document.createElement('canvas');
                    const ctx = canvas.getContext('2d');

                    this._interval = setInterval(async () => {
                        try {
                            if (video.readyState !== video.HAVE_ENOUGH_DATA) return;
                            canvas.width = video.videoWidth;
                            canvas.height = video.videoHeight;
                            ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
                            const bitmap = canvas;
                            const results = await detector.detect(bitmap);
                            if (results && results.length > 0) {
                                const code = results[0].rawValue || results[0].rawData || results[0].rawText || results[0].value;
                                try { await dotNetRef.invokeMethodAsync('OnBarcodeScanned', code); } catch (e) { console.error(e); }
                                this.stop();
                            }
                        } catch (err) {
                            // ignore per-frame errors
                            console.debug('detect err', err);
                        }
                    }, 250);

                    return;
                } catch (bdErr) {
                    console.warn('BarcodeDetector failed, falling back', bdErr);
                    // fallthrough to html5-qrcode below
                    try { if (this._stream) { this._stopStreamTracks(); } } catch (e) { }
                }
            }

            // Fallback: html5-qrcode library
            // load library if needed
            if (typeof Html5Qrcode === 'undefined') {
                await new Promise((resolve, reject) => {
                    const s = document.createElement('script');
                    s.src = 'https://unpkg.com/html5-qrcode@2.3.8/minified/html5-qrcode.min.js';
                    s.onload = resolve;
                    s.onerror = reject;
                    document.head.appendChild(s);
                });
            }

            try {
                this._html5Qrcode = new Html5Qrcode(video);
            } catch (e) {
                // html5-qrcode expects element id or element, some versions accept element
                this._html5Qrcode = new Html5Qrcode(this._elementId);
            }

            const config = { fps: 10, qrbox: { width: 300, height: 100 } };

            try {
                await this._html5Qrcode.start(
                    { facingMode: { exact: 'environment' } },
                    config,
                    (decodedText) => {
                        dotNetRef.invokeMethodAsync('OnBarcodeScanned', decodedText).catch(console.error);
                        this.stop();
                    },
                    (errorMessage) => { /* ignore per-frame errors */ }
                );
            } catch (err) {
                try {
                    await this._html5Qrcode.start({ facingMode: 'environment' }, config,
                        (decodedText) => {
                            dotNetRef.invokeMethodAsync('OnBarcodeScanned', decodedText).catch(console.error);
                            this.stop();
                        });
                } catch (ex) {
                    console.error('Unable to start scanner', ex);
                    dotNetRef.invokeMethodAsync('OnBarcodeError', ex && ex.toString ? ex.toString() : 'error').catch(console.error);
                    this.stop();
                }
            }
        },

        async stop() {
            // stop html5-qrcode if running
            if (this._html5Qrcode) {
                try { await this._html5Qrcode.stop(); } catch (e) { }
                try { this._html5Qrcode.clear(); } catch (e) { }
                this._html5Qrcode = null;
            }

            // stop media stream and interval
            if (this._interval) {
                clearInterval(this._interval);
                this._interval = null;
            }
            if (this._stream) {
                this._stopStreamTracks();
                this._stream = null;
            }

            // remove element
            if (this._elementId) {
                const el = document.getElementById(this._elementId);
                if (el) el.remove();
                this._elementId = null;
            }
        },

        _stopStreamTracks() {
            try {
                if (!this._stream) return;
                const tracks = this._stream.getTracks();
                tracks.forEach(t => { try { t.stop(); } catch (e) { } });
            } catch (e) { }
        }
    };
})();
