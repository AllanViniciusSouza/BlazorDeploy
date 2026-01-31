window.layoutUtils = {
    moveElementToBody: function (id) {
        try {
            var el = document.getElementById(id);
            if (!el) return;
            if (el.parentElement !== document.body) {
                document.body.appendChild(el);
            }
        } catch (e) {
            console.error(e);
        }
    }
    , positionElementWithinContainer: function (id, containerSelector, offset) {
        try {
            var el = document.getElementById(id);
            var container = document.querySelector(containerSelector);
            if (!el || !container) return;

            function apply() {
                var rect = container.getBoundingClientRect();
                // position element fixed aligned to container
                el.style.position = 'fixed';
                el.style.left = (rect.left) + 'px';
                el.style.top = (Math.max(0, rect.top) + (offset || 0)) + 'px';
                el.style.width = (rect.width) + 'px';
                el.style.zIndex = '2000';
            }

            apply();

            // attach resize handler to reposition when viewport changes
            var handlerName = 'layoutUtils_resize_' + id;
            if (window[handlerName]) {
                window.removeEventListener('resize', window[handlerName]);
            }
            window[handlerName] = apply;
            window.addEventListener('resize', window[handlerName]);
        } catch (e) {
            console.error(e);
        }
    }
    
};
