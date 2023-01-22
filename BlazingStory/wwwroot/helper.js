export const setLocalStorageItem = (key, value) => { localStorage.setItem(key, value); };
export const getLocalStorageItem = (key) => (localStorage.getItem(key) || null);
export const copyTextToClipboard = (text) => navigator.clipboard.writeText(text);
const keydown = "keydown";
export const setupKeyDownSender = () => {
    document.addEventListener(keydown, event => {
        window.parent.postMessage({
            action: keydown,
            eventArgs: {
                key: event.key,
                code: event.code,
                altKey: event.altKey,
                shiftKey: event.shiftKey,
                ctrlKey: event.ctrlKey,
                metaKey: event.metaKey,
            }
        }, location.origin);
    });
};
export const setupKeyDownReceiver = () => {
    window.addEventListener("message", (event) => {
        if (event.origin !== location.origin)
            return;
        const message = event.data;
        if (message.action !== keydown)
            return;
        const keydownEvent = new KeyboardEvent(keydown, { ...message.eventArgs, ...{ bubbles: true } });
        document.body.dispatchEvent(keydownEvent);
    }, false);
};
