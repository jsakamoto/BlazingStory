export const setLocalStorageItem = (key, value) => { localStorage.setItem(key, value); };
export const getLocalStorageItem = (key) => (localStorage.getItem(key) || null);
export const copyTextToClipboard = (text) => navigator.clipboard.writeText(text);
export const releaseFocus = () => { var _a; (_a = document.activeElement) === null || _a === void 0 ? void 0 : _a.blur(); };
const keydown = "keydown";
const pointerdown = "pointerdown";
export const setupKeyDownReceiver = () => {
    window.addEventListener("message", (event) => {
        if (event.origin !== location.origin)
            return;
        const message = event.data;
        switch (message.action) {
            case keydown:
                const keydownEvent = new KeyboardEvent(keydown, { ...message.eventArgs, ...{ bubbles: true } });
                document.body.dispatchEvent(keydownEvent);
                break;
            case pointerdown:
                const clickEvent = new MouseEvent(pointerdown, { bubbles: true });
                document.body.dispatchEvent(clickEvent);
                break;
        }
    }, false);
};
