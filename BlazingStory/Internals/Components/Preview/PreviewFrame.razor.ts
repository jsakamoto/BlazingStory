import type { FrameHeightChangeEvent, ComponentActionEvent } from "../../../wwwroot/js/lib";
import type { CSSStyle, MessageArgument } from "../../../Scripts/types";

// Register custom event types
Blazor?.registerCustomEventType('frameheightchange', {
    createEventArgs: (e: FrameHeightChangeEvent) => e.detail
});
Blazor?.registerCustomEventType('componentaction', {
    createEventArgs: (e: ComponentActionEvent) => e.detail
});

const waitFor = async <T>(arg: { predecate: () => false | T, maxRetryCount?: number, retryInterval?: number }): Promise<T | null> => {
    let retryCount = 0;
    while (true) {
        const result = arg.predecate();
        if (result !== false) return result as T;
        if (retryCount >= (arg.maxRetryCount ?? 500)) return null;
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
            return { contentWindow: iframe.contentWindow, contentDocument: iframe.contentDocument };
        }
    });
}

export const reloadPreviewFrame = async (container: HTMLElement): Promise<void> => {
    const result = await getIFrame(container);
    result?.contentWindow.postMessage({ action: "reload" } as MessageArgument);
}

const zoomPreviewFrame = async (container: HTMLElement, getNextZoomLevel: (zoomLevel: number) => number): Promise<void> => {
    const result = await getIFrame(container);
    if (!result) return;
    const style = result.contentDocument.body.style as CSSStyle;
    const currentZoomLevel = parseFloat(style.zoom || '1');
    const nextZoomLevel = getNextZoomLevel(currentZoomLevel);
    style.zoom = '' + nextZoomLevel;
}

export const zoomInPreviewFrame = (container: HTMLElement) => zoomPreviewFrame(container, zoom => zoom * 1.25);

export const zoomOutPreviewFrame = (container: HTMLElement) => zoomPreviewFrame(container, zoom => zoom / 1.25);

export const resetZoomPreviewFrame = (container: HTMLElement) => zoomPreviewFrame(container, _ => 1);

export const getFrameHeight = async (container: HTMLElement): Promise<number> => {
    const result = await getIFrame(container);
    return Math.ceil(result?.contentDocument.body.parentElement?.getBoundingClientRect().height || 0);
}

// Checks the '_dotnet_watch_ws_injected' property has been added to the window object,
// indicating that the '_framework/aspnetcore-browser-refresh.js' script has been loaded.
// see: https://github.com/dotnet/sdk/blob/12c083fc90700d3255cc021b665764876c5747fe/src/BuiltInTools/BrowserRefresh/WebSocketScriptInjection.js#L4
const isDotnetWatchScriptInjected = (window: Window | null): boolean => {
    const scriptInjectedSentinel = '_dotnet_watch_ws_injected';
    return window?.hasOwnProperty(scriptInjectedSentinel) ?? false;
}

export const ensureDotnetWatchScriptInjected = async (container: HTMLElement): Promise<void> => {
    const result = await getIFrame(container);
    if (!result) return;
    const { contentWindow, contentDocument } = result;

    if (!isDotnetWatchScriptInjected(window))
        return; // Hot reloading is not available
    if (isDotnetWatchScriptInjected(contentWindow))
        return; // Already injected

    const script = contentDocument.createElement('script');
    script.src = '_framework/aspnetcore-browser-refresh.js';
    contentDocument.body.appendChild(script);
}

