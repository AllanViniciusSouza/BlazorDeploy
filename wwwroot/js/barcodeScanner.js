// barcodeScanner helper - loads html5-qrcode dynamically and scans barcode
(function () {
    window.barcodeScanner = {
        _html5Qrcode: null,
        _elementId: null,
        async start(dotNetRef, elementId) {
            this._elementId = elementId;
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

            // ensure target element exists
            let container = document.getElementById(elementId);
            if (!container) {
                container = document.createElement('div');
                container.id = elementId;
                container.style.width = '100%';
                container.style.maxWidth = '400px';
                container.style.margin = '0 auto';
                document.body.appendChild(container);
            }

            // create scanner
            this._html5Qrcode = new Html5Qrcode(elementId);

            const config = { fps: 10, qrbox: { width: 300, height: 100 } };

            try {
                await this._html5Qrcode.start(
                    { facingMode: { exact: "environment" } },
                    config,
                    (decodedText, decodedResult) => {
                        // stop after first decode
                        try {
                            dotNetRef.invokeMethodAsync('OnBarcodeScanned', decodedText);
                        } catch (e) { console.error(e); }
                        this.stop();
                    },
                    (errorMessage) => {
                        // ignore per-frame decode errors
                    }
                );
            } catch (err) {
                // fallback to any camera
                try {
                    await this._html5Qrcode.start({ facingMode: 'environment' }, config,
                        (decodedText) => {
                            dotNetRef.invokeMethodAsync('OnBarcodeScanned', decodedText);
                            this.stop();
                        });
                } catch (ex) {
                    console.error('Unable to start scanner', ex);
                    dotNetRef.invokeMethodAsync('OnBarcodeError', ex && ex.toString ? ex.toString() : 'error');
                }
            }
        },
        async stop() {
            if (this._html5Qrcode) {
                try {
                    await this._html5Qrcode.stop();
                } catch (e) { }
                this._html5Qrcode.clear();
                this._html5Qrcode = null;
            }
            // remove element
            if (this._elementId) {
                const el = document.getElementById(this._elementId);
                if (el) el.remove();
                this._elementId = null;
            }
        }
    };
})();
