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
const getIFrame = async (container) => {
    return await waitFor({
        predecate: () => {
            const iframe = container.querySelector('iframe');
            if (!iframe)
                return false;
            if (!iframe.contentWindow)
                return false;
            if (!iframe.contentDocument)
                return false;
            if (iframe.contentWindow.location.href === "about:blank")
                return false;
            if (iframe.contentWindow.BlazingStory?.canvasFrameInitialized !== true)
                return false;
            return { iframe, contentWindow: iframe.contentWindow, contentDocument: iframe.contentDocument };
        }
    });
};
export const reloadPreviewFrame = async (container) => {
    const { contentWindow } = await getIFrame(container);
    contentWindow.postMessage({ action: "reload" });
};
const zoomPreviewFrame = async (container, getNextZoomLevel) => {
    const { contentDocument } = await getIFrame(container);
    const style = contentDocument.body.style;
    const currentZoomLevel = parseFloat(style.zoom || '1');
    const nextZoomLevel = getNextZoomLevel(currentZoomLevel);
    style.zoom = '' + nextZoomLevel;
};
export const zoomInPreviewFrame = (container) => zoomPreviewFrame(container, zoom => zoom * 1.25);
export const zoomOutPreviewFrame = (container) => zoomPreviewFrame(container, zoom => zoom / 1.25);
export const resetZoomPreviewFrame = (container) => zoomPreviewFrame(container, _ => 1);
export const getFrameScrollHeight = async (container) => {
    const { contentDocument } = await getIFrame(container);
    return contentDocument.body.parentElement?.scrollHeight || 0;
};
export const subscribeComponentActionEvent = async (container, dotNetObj, methodName) => {
    try {
        const { contentDocument } = await getIFrame(container);
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
export const ensureDotnetWatchScriptInjected = async (container) => {
    try {
        const { contentWindow, contentDocument } = await getIFrame(container);
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
