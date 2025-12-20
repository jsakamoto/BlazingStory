class TimeoutError extends Error {
    constructor(message) { super(message); }
}
const waitFor = async (arg) => {
    let retryCount = 0;
    while (true) {
        const result = arg.predecate();
        if (result !== false)
            return result;
        if (retryCount >= (arg.maxRetryCount ?? 500))
            throw new TimeoutError("Timeout");
        retryCount++;
        await new Promise(resolve => setTimeout(resolve, arg.retryInterval ?? 10));
    }
};
const waitForIFrameReady = async (iframe) => {
    return await waitFor({
        predecate: () => {
            if (!iframe.contentDocument)
                return false;
            if (!iframe.contentWindow)
                return false;
            if (iframe.contentWindow.location.href === "about:blank")
                return false;
            if (iframe.contentWindow.BlazingStory?.canvasFrameInitialized !== true)
                return false;
            return ({ contentWindow: iframe.contentWindow, contentDocument: iframe.contentDocument });
        }
    });
};
export const navigatePreviewFrameTo = async (iframe, url) => {
    if (!(iframe instanceof HTMLIFrameElement))
        return;
    const { contentWindow, contentDocument } = await waitForIFrameReady(iframe);
    const event = new PopStateEvent("popstate", { state: {}, bubbles: true, cancelable: true });
    contentWindow.history.pushState({}, "", url);
    contentDocument.dispatchEvent(event);
};
export const reloadPreviewFrame = (iframe) => {
    if (iframe === null || iframe.contentWindow === null)
        return;
    iframe.contentWindow.postMessage({ action: "reload" });
};
const zoomPreviewFrame = (iframe, getNextZoomLevel) => {
    if (iframe === null || iframe.contentDocument === null)
        return;
    const style = iframe.contentDocument.body.style;
    const currentZoomLevel = parseFloat(style.zoom || '1');
    const nextZoomLevel = getNextZoomLevel(currentZoomLevel);
    style.zoom = '' + nextZoomLevel;
};
export const zoomInPreviewFrame = (iframe) => zoomPreviewFrame(iframe, zoom => zoom * 1.25);
export const zoomOutPreviewFrame = (iframe) => zoomPreviewFrame(iframe, zoom => zoom / 1.25);
export const resetZoomPreviewFrame = (iframe) => zoomPreviewFrame(iframe, _ => 1);
export const subscribeComponentActionEvent = async (iframe, dotNetObj, methodName) => {
    try {
        if (iframe === null)
            return { dispose: () => { } };
        const { contentDocument } = await waitForIFrameReady(iframe);
        const componentActionEventListener = (e) => dotNetObj.invokeMethodAsync(methodName, e.detail.name, e.detail.argsJson);
        contentDocument.addEventListener('componentActionEvent', componentActionEventListener);
        return { dispose: () => contentDocument.removeEventListener('componentActionEvent', componentActionEventListener) };
    }
    catch (e) {
        if (e instanceof TimeoutError)
            return { dispose: () => { } };
        throw e;
    }
};
const isDotnetWatchScriptInjected = (window) => {
    const scriptInjectedSentinel = '_dotnet_watch_ws_injected';
    return window?.hasOwnProperty(scriptInjectedSentinel) ?? false;
};
export const ensureDotnetWatchScriptInjected = async (iframe) => {
    try {
        if (iframe === null)
            return;
        const { contentWindow, contentDocument } = await waitForIFrameReady(iframe);
        if (!isDotnetWatchScriptInjected(window))
            return;
        if (isDotnetWatchScriptInjected(contentWindow))
            return;
        const script = contentDocument.createElement('script');
        script.src = '_framework/aspnetcore-browser-refresh.js';
        contentDocument.body.appendChild(script);
    }
    catch (e) {
        if (e instanceof TimeoutError)
            return;
        throw e;
    }
};
