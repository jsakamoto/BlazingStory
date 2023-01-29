export const navigateCanvasFrameTo = (iframe, url) => {
    if (iframe.contentWindow === null || iframe.contentDocument === null)
        return;
    const event = new PopStateEvent("popstate", { state: {}, bubbles: true, cancelable: true });
    iframe.contentWindow.history.pushState({}, "", url);
    iframe.contentDocument.dispatchEvent(event);
};
export const reloadCanvasFrame = (iframe) => {
    if (iframe.contentWindow === null)
        return;
    iframe.contentWindow.postMessage({ action: "reload" });
};
const zoomCanvasFrame = (iframe, getNextZoomLevel) => {
    if (iframe.contentDocument === null)
        return;
    const style = iframe.contentDocument.body.style;
    const currentZoomLevel = parseFloat(style.zoom || '1');
    const nextZoomLevel = getNextZoomLevel(currentZoomLevel);
    style.zoom = '' + nextZoomLevel;
};
export const zoomInCanvasFrame = (iframe) => zoomCanvasFrame(iframe, zoom => zoom * 1.25);
export const zoomOutCanvasFrame = (iframe) => zoomCanvasFrame(iframe, zoom => zoom / 1.25);
export const resetZoomCanvasFrame = (iframe) => zoomCanvasFrame(iframe, _ => 1);
