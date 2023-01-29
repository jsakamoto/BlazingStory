import { CSSStyle, MessageArgument } from "../../../Scripts/types";

export const navigateCanvasFrameTo = (iframe: HTMLIFrameElement, url: string) => {
    if (iframe.contentWindow === null || iframe.contentDocument === null) return;
    const event = new PopStateEvent("popstate", { state: {}, bubbles: true, cancelable: true });
    iframe.contentWindow.history.pushState({}, "", url);
    iframe.contentDocument.dispatchEvent(event);
}

export const reloadCanvasFrame = (iframe: HTMLIFrameElement) => {
    if (iframe.contentWindow === null) return;
    iframe.contentWindow.postMessage({ action: "reload" } as MessageArgument);
}

const zoomCanvasFrame = (iframe: HTMLIFrameElement, getNextZoomLevel: (zoomLevel: number) => number) => {
    if (iframe.contentDocument === null) return;
    const style = iframe.contentDocument.body.style as CSSStyle;
    const currentZoomLevel = parseFloat(style.zoom || '1');
    const nextZoomLevel = getNextZoomLevel(currentZoomLevel);
    style.zoom = '' + nextZoomLevel;
}

export const zoomInCanvasFrame = (iframe: HTMLIFrameElement) => zoomCanvasFrame(iframe, zoom => zoom * 1.25);

export const zoomOutCanvasFrame = (iframe: HTMLIFrameElement) => zoomCanvasFrame(iframe, zoom => zoom / 1.25);

export const resetZoomCanvasFrame = (iframe: HTMLIFrameElement) => zoomCanvasFrame(iframe, _ => 1);
