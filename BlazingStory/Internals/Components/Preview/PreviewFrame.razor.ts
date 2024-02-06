import { CSSStyle, MessageArgument, DotNetObjectReference } from "../../../Scripts/types";

const waitFor = async <T>(arg: { predecate: () => false | T, maxRetryCount?: number, retryInterval?: number }): Promise<T> => {
    const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));
    let retryCount = 0;
    while (true) {
        const result = arg.predecate();
        if (result !== false) return result as T;
        if (retryCount >= (arg.maxRetryCount ?? 50)) throw new Error("Timeout");
        retryCount++;
        await delay(arg.retryInterval ?? 10);
    }
}

const waitForIFrameReady = async <T>(iframe: HTMLIFrameElement) => {
    return await waitFor({
        predecate: () => {
            if (iframe.contentWindow === null || iframe.contentDocument === null) return false;
            if (iframe.contentWindow.location.href === "about:blank") return false;
            if (iframe.contentWindow.BlazingStory?.canvasFrameInitialized !== true) return false;
            return ({ contentWindow: iframe.contentWindow, contentDocument: iframe.contentDocument });
        }
    });
}

export const navigatePreviewFrameTo = async (iframe: HTMLIFrameElement | null, url: string) => {
    if (iframe === null) return;
    const { contentWindow, contentDocument } = await waitForIFrameReady(iframe);
    const event = new PopStateEvent("popstate", { state: {}, bubbles: true, cancelable: true });
    contentWindow.history.pushState({}, "", url);
    contentDocument.dispatchEvent(event);
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

export const subscribeComponentActionEvent = async (iframe: HTMLIFrameElement | null, dotNetObj: DotNetObjectReference, methodName: string) => {
    if (iframe === null) return;
    const { contentDocument } = await waitForIFrameReady(iframe);
    const componentActionEventListener = (e: Event & { detail?: any }) => dotNetObj.invokeMethodAsync(methodName, e.detail.name, e.detail.argsJson);
    contentDocument.addEventListener('componentActionEvent', componentActionEventListener);

    return { dispose: () => contentDocument.removeEventListener('componentActionEvent', componentActionEventListener) };
}

// Checks the '_dotnet_watch_ws_injected' property has been added to the window object,
// indicating that the '_framework/aspnetcore-browser-refresh.js' script has been loaded.
// see: https://github.com/dotnet/sdk/blob/12c083fc90700d3255cc021b665764876c5747fe/src/BuiltInTools/BrowserRefresh/WebSocketScriptInjection.js#L4
const isDotnetWatchScriptInjected = (window: Window | null): boolean => {
    const scriptInjectedSentinel = '_dotnet_watch_ws_injected';
    return window?.hasOwnProperty(scriptInjectedSentinel) ?? false;
}

export const ensureDotnetWatchScriptInjected = async (iframe: HTMLIFrameElement | null): Promise<void> => {
    if (iframe === null) return;

    const { contentWindow, contentDocument } = await waitForIFrameReady(iframe);

    if (!isDotnetWatchScriptInjected(window))
        return; // Hot reloading is not available
    if (isDotnetWatchScriptInjected(contentWindow))
        return; // Already injected

    const script = contentDocument.createElement('script');
    script.src = '_framework/aspnetcore-browser-refresh.js';
    contentDocument.body.appendChild(script);
}

