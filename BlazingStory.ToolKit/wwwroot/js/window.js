const getTargetWindow = (selector) => {
    if (selector === "parent")
        return window.parent;
    const iframe = document.querySelector(selector);
    if (iframe instanceof HTMLIFrameElement && iframe.contentWindow)
        return iframe.contentWindow;
    throw new Error(`No target window found for selector: ${selector}`);
};
export const postMessage = (selector, message) => {
    const targetWindow = getTargetWindow(selector);
    targetWindow.postMessage(message, location.origin);
};
export const setupMessageListener = (obj) => {
    const handler = (event) => {
        if (event.origin !== location.origin)
            return;
        const message = typeof (event.data) === "string" ? event.data : JSON.stringify(event.data);
        obj.invokeMethodAsync("OnMessageReceived", message);
    };
    window.addEventListener("message", handler);
    return {
        dispose: () => {
            window.removeEventListener("message", handler);
        }
    };
};
