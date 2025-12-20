import { CSSStyle, MessageArgument, DotNetObjectReference } from "../../../Scripts/types";

class TimeoutError extends Error {
    constructor(message: string) { super(message); }
}

const waitFor = async <T>(arg: { predecate: () => false | T, maxRetryCount?: number, retryInterval?: number }): Promise<T> => {
    let retryCount = 0;
    while (true) {
        const result = arg.predecate();
        if (result !== false) return result as T;
        if (retryCount >= (arg.maxRetryCount ?? 500)) throw new TimeoutError("Timeout");
        retryCount++;
        await new Promise(resolve => setTimeout(resolve, arg.retryInterval ?? 10));
    }
}

const getIFrame = async (container: HTMLElement) => {
    return await waitFor({
        predecate: () => {
            const iframe = container.querySelector('iframe');
            if (!iframe) return false;
            if (!iframe.contentWindow) return false;
            if (!iframe.contentDocument) return false;
            if (iframe.contentWindow.location.href === "about:blank") return false;
            if (iframe.contentWindow.BlazingStory?.canvasFrameInitialized !== true) return false;
            return { iframe, contentWindow: iframe.contentWindow, contentDocument: iframe.contentDocument };
        }
    });
}

export const reloadPreviewFrame = async (container: HTMLElement): Promise<void> => {
    const { contentWindow } = await getIFrame(container);
    contentWindow.postMessage({ action: "reload" } as MessageArgument);
}

const zoomPreviewFrame = async (container: HTMLElement, getNextZoomLevel: (zoomLevel: number) => number): Promise<void> => {
    const { contentDocument } = await getIFrame(container);
    const style = contentDocument.body.style as CSSStyle;
    const currentZoomLevel = parseFloat(style.zoom || '1');
    const nextZoomLevel = getNextZoomLevel(currentZoomLevel);
    style.zoom = '' + nextZoomLevel;
}

export const zoomInPreviewFrame = (container: HTMLElement) => zoomPreviewFrame(container, zoom => zoom * 1.25);

export const zoomOutPreviewFrame = (container: HTMLElement) => zoomPreviewFrame(container, zoom => zoom / 1.25);

export const resetZoomPreviewFrame = (container: HTMLElement) => zoomPreviewFrame(container, _ => 1);

export const subscribeComponentActionEvent = async (container: HTMLElement, dotNetObj: DotNetObjectReference, methodName: string) => {
    try {
        const { contentDocument } = await getIFrame(container);
        const componentActionEventListener = (e: Event & { detail?: any }) => dotNetObj.invokeMethodAsync(methodName, e.detail.name, e.detail.argsJson);
        contentDocument.addEventListener('componentActionEvent', componentActionEventListener);

        return { dispose: () => contentDocument.removeEventListener('componentActionEvent', componentActionEventListener) };
    }
    catch (e) {
        if (e instanceof TimeoutError) return { dispose: () => { } };
        throw e;
    }
}

// Checks the '_dotnet_watch_ws_injected' property has been added to the window object,
// indicating that the '_framework/aspnetcore-browser-refresh.js' script has been loaded.
// see: https://github.com/dotnet/sdk/blob/12c083fc90700d3255cc021b665764876c5747fe/src/BuiltInTools/BrowserRefresh/WebSocketScriptInjection.js#L4
const isDotnetWatchScriptInjected = (window: Window | null): boolean => {
    const scriptInjectedSentinel = '_dotnet_watch_ws_injected';
    return window?.hasOwnProperty(scriptInjectedSentinel) ?? false;
}

export const ensureDotnetWatchScriptInjected = async (container: HTMLElement): Promise<void> => {
    try {
        const { contentWindow, contentDocument } = await getIFrame(container);

        if (!isDotnetWatchScriptInjected(window))
            return; // Hot reloading is not available
        if (isDotnetWatchScriptInjected(contentWindow))
            return; // Already injected

        const script = contentDocument.createElement('script');
        script.src = '_framework/aspnetcore-browser-refresh.js';
        contentDocument.body.appendChild(script);
    }
    catch (e) {
        if (e instanceof TimeoutError) return;
        throw e;
    }
}

