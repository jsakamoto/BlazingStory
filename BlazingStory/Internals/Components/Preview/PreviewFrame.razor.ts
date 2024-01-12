import { CSSStyle, MessageArgument } from "../../../Scripts/types";

const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

const waitFor = async (arg: { predecate: () => boolean, maxRetryCount: number, retryInterval: number }): Promise<void> => {
    let retryCount = 0;
    while (true) {
        if (arg.predecate()) return;
        if (retryCount >= arg.maxRetryCount) throw new Error("Timeout");
        retryCount++;
        await delay(arg.retryInterval);
    }
}

export const navigatePreviewFrameTo = async (iframe: HTMLIFrameElement | null, url: string) => {
    if (iframe === null) return;
    await waitFor({
        predecate: () => {
            if (iframe.contentWindow === null || iframe.contentDocument === null) return false;
            if (iframe.contentWindow.location.href === "about:blank") return false;
            if (iframe.contentWindow.BlazingStory?.canvasFrameInitialized !== true) return false;
            return true;
        },
        maxRetryCount: 50,
        retryInterval: 10
    });

    const event = new PopStateEvent("popstate", { state: {}, bubbles: true, cancelable: true });
    iframe.contentWindow!.history.pushState({}, "", url);
    iframe.contentDocument!.dispatchEvent(event);
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

// Checks the '_dotnet_watch_ws_injected' property has been added to the window object,
// indicating that the '_framework/aspnetcore-browser-refresh.js' script has been loaded.
// see: https://github.com/dotnet/sdk/blob/12c083fc90700d3255cc021b665764876c5747fe/src/BuiltInTools/BrowserRefresh/WebSocketScriptInjection.js#L4
const isDotnetWatchScriptInjected = (window: Window | null):boolean => {
    const scriptInjectedSentinel = '_dotnet_watch_ws_injected';
    return window?.hasOwnProperty(scriptInjectedSentinel) ?? false;
}

// Checks if hot reload is enabled by verifying if the dotnet watch script is injected in the current window.
export const isHotReloadEnabled = ():boolean => {
    return isDotnetWatchScriptInjected(window);
}

export const ensureDotnetWatchScriptInjected = (iframe: HTMLIFrameElement | null):void => {
    if (iframe === null || iframe.contentWindow == null || iframe.contentDocument == null) return;
    if (isDotnetWatchScriptInjected(iframe.contentWindow)) 
        return; // Already injected

    const script = iframe.contentDocument!.createElement('script');
    script.src = '_framework/aspnetcore-browser-refresh.js';
    iframe.contentDocument!.body.appendChild(script);
}


export const zoomInPreviewFrame = (iframe: HTMLIFrameElement | null) => zoomPreviewFrame(iframe, zoom => zoom * 1.25);

export const zoomOutPreviewFrame = (iframe: HTMLIFrameElement | null) => zoomPreviewFrame(iframe, zoom => zoom / 1.25);

export const resetZoomPreviewFrame = (iframe: HTMLIFrameElement | null) => zoomPreviewFrame(iframe, _ => 1);
