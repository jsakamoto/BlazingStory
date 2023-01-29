export const navigateCanvasFrameTo = (iframe, url) => {
    const event = new PopStateEvent("popstate", { state: {}, bubbles: true, cancelable: true });
    iframe.contentWindow.history.pushState({}, "", url);
    iframe.contentDocument.dispatchEvent(event);
}

export const reloadCanvasFrame = (iframe) => {
    iframe.contentWindow.location.reload(true);
}