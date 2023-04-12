import { CSSStyle, MessageArgument } from "../../../Scripts/types";

export const navigatePreviewFrameTo = (iframe: HTMLIFrameElement | null, url: string) => {
    if (iframe === null || iframe.contentWindow === null || iframe.contentDocument === null) return;
    const event = new PopStateEvent("popstate", { state: {}, bubbles: true, cancelable: true });
    iframe.contentWindow.history.pushState({}, "", url);
    iframe.contentDocument.dispatchEvent(event);
}

export const reloadPreviewFrame = (iframe: HTMLIFrameElement | null) => {
    if (iframe === null || iframe.contentWindow === null) return;
    iframe.contentWindow.postMessage({ action: "reload" } as MessageArgument);
}

const zoomPreviewFrame = (iframe: HTMLIFrameElement | null, getNextZoomLevel: (zoomLevel: number) => number) => {
    if (iframe === null || iframe.contentDocument === null) return;
    const style = iframe.contentDocument.body.style as CSSStyle;
    const currentZoomLevel = parseFloat(style.zoom || '1');
    const nextZoomLevel = getNextZoomLevel(currentZoomLevel);
    style.zoom = '' + nextZoomLevel;
}

export const zoomInPreviewFrame = (iframe: HTMLIFrameElement | null) => zoomPreviewFrame(iframe, zoom => zoom * 1.25);

export const zoomOutPreviewFrame = (iframe: HTMLIFrameElement | null) => zoomPreviewFrame(iframe, zoom => zoom / 1.25);

export const resetZoomPreviewFrame = (iframe: HTMLIFrameElement | null) => zoomPreviewFrame(iframe, _ => 1);
