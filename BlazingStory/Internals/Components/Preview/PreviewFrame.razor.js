const delay = (ms) => new Promise(resolve => setTimeout(resolve, ms));
const waitFor = async (arg) => {
    let retryCount = 0;
    while (true) {
        if (arg.predecate())
            return;
        if (retryCount >= arg.maxRetryCount)
            throw new Error("Timeout");
        retryCount++;
        await delay(arg.retryInterval);
    }
};
export const navigatePreviewFrameTo = async (iframe, url) => {
    if (iframe === null)
        return;
    await waitFor({
        predecate: () => iframe.contentWindow !== null && iframe.contentDocument !== null && iframe.contentWindow.location.href !== "about:blank",
        maxRetryCount: 50,
        retryInterval: 10
    });
    const event = new PopStateEvent("popstate", { state: {}, bubbles: true, cancelable: true });
    iframe.contentWindow.history.pushState({}, "", url);
    iframe.contentDocument.dispatchEvent(event);
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
