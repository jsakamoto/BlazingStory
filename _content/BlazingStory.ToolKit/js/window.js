const getTargetWindow = async (selector) => {
    if (selector === "parent")
        return window.parent;
    for (let i = 0; i < 30; i++) {
        const iframe = document.querySelector(selector);
        if (iframe instanceof HTMLIFrameElement && iframe.contentWindow)
            return iframe.contentWindow;
        await new Promise(resolve => setTimeout(resolve, 100));
    }
    throw new Error(`No target window found for selector: ${selector}`);
};
export const postMessage = async (selector, message) => {
    const targetWindow = await getTargetWindow(selector);
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
