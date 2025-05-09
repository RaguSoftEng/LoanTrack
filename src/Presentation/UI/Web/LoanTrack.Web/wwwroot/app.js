window.getBoundingClientRect = (element) => {
    if (!element) return null;
    let rect = element.getBoundingClientRect();
    return {
        top: rect.top,
        bottom: rect.bottom,
        height: rect.height,
        windowHeight: window.innerHeight
    };
};

window.searchDropdown = {
    registerClickAway: function (componentElement, dotNetRef) {
        const handler = (e) => {
            if (!componentElement.contains(e.target)) {
                dotNetRef.invokeMethodAsync('HideDropdown');
                document.removeEventListener('click', handler);
            }
        };
        setTimeout(() => document.addEventListener('click', handler), 0);
    }
};

window.modalHelper = {
    show: function (id) {
        const el = document.getElementById(id);
        if (el) {
            const modal = new bootstrap.Modal(el);
            modal.show();
        }
    },
    hide: function (id) {
        const el = document.getElementById(id);
        if (el) {
            const modal = bootstrap.Modal.getInstance(el);
            if (modal) modal.hide();
        }
    }
};