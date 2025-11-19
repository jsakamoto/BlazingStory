const doc = document;
const lstorage = localStorage;
export const setLocalStorageItem = (key, value) => { lstorage.setItem(key, value); };
export const getLocalStorageItem = (key) => (lstorage.getItem(key) || null);
export const copyTextToClipboard = (text) => navigator.clipboard.writeText(text);
export const releaseFocus = () => { var _a; (_a = doc.activeElement) === null || _a === void 0 ? void 0 : _a.blur(); };
const keydown = "keydown";
const pointerdown = "pointerdown";
export const setupMessageReceiverFromIFrame = () => {
    window.addEventListener("message", (event) => {
        if (event.origin !== location.origin)
            return;
        const message = event.data;
        switch (message.action) {
            case keydown:
                const keydownEvent = new KeyboardEvent(keydown, { ...message.eventArgs, ...{ bubbles: true } });
                doc.body.dispatchEvent(keydownEvent);
                break;
            case pointerdown:
                const clickEvent = new MouseEvent(pointerdown, { bubbles: true });
                doc.body.dispatchEvent(clickEvent);
                break;
            case "frameview-height":
                const previewFrameViewPort = doc.querySelector(`.preview-frame-viewport:has(iframe#${message.frameId})`);
                if (previewFrameViewPort) {
                    previewFrameViewPort.style.height = message.height + "px";
                }
                break;
        }
    }, false);
};
