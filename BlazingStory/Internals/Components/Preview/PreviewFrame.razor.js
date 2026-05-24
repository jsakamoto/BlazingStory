Blazor?.registerCustomEventType('frameheightchange', {
    createEventArgs: (e) => e.detail
});
const waitFor = async (arg) => {
    let retryCount = 0;
    while (true) {
        const result = arg.predecate();
        if (result !== false)
            return result;
        if (retryCount >= (arg.maxRetryCount ?? 500))
            return null;
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
            return { contentWindow: iframe.contentWindow, contentDocument: iframe.contentDocument };
        }
    });
};
export const reloadPreviewFrame = async (container) => {
    const result = await getIFrame(container);
    result?.contentWindow.postMessage({ action: "reload" });
};
const zoomPreviewFrame = async (container, getNextZoomLevel) => {
    const result = await getIFrame(container);
    const body = result?.contentDocument.body;
    if (!body)
        return;
    const style = window.getComputedStyle(body);
    const currentZoomLevel = parseFloat(style.getPropertyValue('--bs-zoom') || style.zoom || '1');
    const nextZoomLevel = getNextZoomLevel(currentZoomLevel);
    body.style.setProperty('--bs-zoom', '' + nextZoomLevel);
    body.style.zoom = 'var(--bs-zoom, 1)';
};
export const zoomInPreviewFrame = (container) => zoomPreviewFrame(container, zoom => zoom * 1.25);
export const zoomOutPreviewFrame = (container) => zoomPreviewFrame(container, zoom => zoom / 1.25);
export const resetZoomPreviewFrame = (container) => zoomPreviewFrame(container, _ => 1);
export const getFrameHeight = async (container) => {
    const result = await getIFrame(container);
    return Math.ceil(result?.contentDocument.body.parentElement?.getBoundingClientRect().height || 0);
};
const isDotnetWatchScriptInjected = (window) => {
    const scriptInjectedSentinel = '_dotnet_watch_ws_injected';
    return window?.hasOwnProperty(scriptInjectedSentinel) ?? false;
};
export const ensureDotnetWatchScriptInjected = async (container) => {
    const result = await getIFrame(container);
    if (!result)
        return;
    const { contentWindow, contentDocument } = result;
    if (!isDotnetWatchScriptInjected(window))
        return;
    if (isDotnetWatchScriptInjected(contentWindow))
        return;
    const script = contentDocument.createElement('script');
    script.src = '_framework/aspnetcore-browser-refresh.js';
    contentDocument.body.appendChild(script);
};
