const waitFor = async (arg) => {
    var _a, _b;
    const delay = (ms) => new Promise(resolve => setTimeout(resolve, ms));
    let retryCount = 0;
    while (true) {
        const result = arg.predecate();
        if (result !== false)
            return result;
        if (retryCount >= ((_a = arg.maxRetryCount) !== null && _a !== void 0 ? _a : 50))
            throw new Error("Timeout");
        retryCount++;
        await delay((_b = arg.retryInterval) !== null && _b !== void 0 ? _b : 10);
    }
};
const waitForIFrameReady = async (iframe) => {
    return await waitFor({
        predecate: () => {
            var _a;
            if (iframe.contentWindow === null || iframe.contentDocument === null)
                return false;
            if (iframe.contentWindow.location.href === "about:blank")
                return false;
            if (((_a = iframe.contentWindow.BlazingStory) === null || _a === void 0 ? void 0 : _a.canvasFrameInitialized) !== true)
                return false;
            return ({ contentWindow: iframe.contentWindow, contentDocument: iframe.contentDocument });
        }
    });
};
export const navigatePreviewFrameTo = async (iframe, url) => {
    if (iframe === null)
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
export const subscribeComponentEvent = async (iframe, dotNetObj, methodName) => {
    if (iframe === null)
        return;
    const { contentDocument } = await waitForIFrameReady(iframe);
    const componentEventListener = (e) => dotNetObj.invokeMethodAsync(methodName, e.detail.name, e.detail.argsJson);
    contentDocument.addEventListener('componentEvent', componentEventListener);
    return { dispose: () => contentDocument.removeEventListener('componentEvent', componentEventListener) };
};
const isDotnetWatchScriptInjected = (window) => {
    var _a;
    const scriptInjectedSentinel = '_dotnet_watch_ws_injected';
    return (_a = window === null || window === void 0 ? void 0 : window.hasOwnProperty(scriptInjectedSentinel)) !== null && _a !== void 0 ? _a : false;
};
export const ensureDotnetWatchScriptInjected = async (iframe) => {
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
};
