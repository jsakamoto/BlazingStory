import type {} from "@blazingstory/types/browser-dom";
import type { MessageArgument } from "@blazingstory/types/custom-messages";
import type { FrameHeightChangeEvent } from "@blazingstory/types/custom-events";

// Register custom event types
Blazor?.registerCustomEventType("frameheightchange", {
    createEventArgs: (e: FrameHeightChangeEvent) => e.detail,
});

const waitFor = async <T>(arg: {
    predecate: () => false | T;
    maxRetryCount?: number;
    retryInterval?: number;
}): Promise<T | null> => {
    let retryCount = 0;
    while (true) {
        const result = arg.predecate();
        if (result !== false) return result as T;
        if (retryCount >= (arg.maxRetryCount ?? 500)) return null;
        retryCount++;
        await new Promise((resolve) =>
            setTimeout(resolve, arg.retryInterval ?? 10),
        );
    }
};

const getIFrame = async (container: HTMLElement) => {
    return await waitFor({
        predecate: () => {
            const iframe = container.querySelector("iframe");
            if (!iframe) return false;
            if (!iframe.contentWindow) return false;
            if (!iframe.contentDocument) return false;
            if (iframe.contentWindow.location.href === "about:blank")
                return false;
            if (
                iframe.contentWindow.BlazingStory?.canvasFrameInitialized !==
                true
            )
                return false;
            return {
                contentWindow: iframe.contentWindow,
                contentDocument: iframe.contentDocument,
            };
        },
    });
};

export const reloadPreviewFrame = async (
    container: HTMLElement,
): Promise<void> => {
    const result = await getIFrame(container);
    result?.contentWindow.postMessage({ action: "reload" } as MessageArgument);
};

const zoomPreviewFrame = async (
    container: HTMLElement,
    getNextZoomLevel: (zoomLevel: number) => number,
): Promise<void> => {
    const result = await getIFrame(container);
    const body = result?.contentDocument.body;
    if (!body) return;
    const style = window.getComputedStyle(body);
    const currentZoomLevel = parseFloat(
        style.getPropertyValue("--bs-zoom") || style.zoom || "1",
    );
    const nextZoomLevel = getNextZoomLevel(currentZoomLevel);
    result.contentWindow.postMessage({
        action: "zoom",
        zoomLevel: nextZoomLevel,
    } as MessageArgument);
};

export const zoomInPreviewFrame = (container: HTMLElement) =>
    zoomPreviewFrame(container, (zoom) => zoom * 1.25);

export const zoomOutPreviewFrame = (container: HTMLElement) =>
    zoomPreviewFrame(container, (zoom) => zoom / 1.25);

export const resetZoomPreviewFrame = (container: HTMLElement) =>
    zoomPreviewFrame(container, (_) => 1);

export const getFrameHeight = async (
    container: HTMLElement,
): Promise<number> => {
    const result = await getIFrame(container);
    return Math.ceil(
        result?.contentDocument.body.parentElement?.getBoundingClientRect()
            .height || 0,
    );
};

export const getFrameHeightFromIFrame = async (
    iframe: HTMLIFrameElement,
): Promise<number> => {
    const html = iframe.contentDocument?.body?.parentElement;
    return Math.ceil(html?.getBoundingClientRect().height || 0);
};

export const navigatePreviewFrameTo = (
    iframe: HTMLIFrameElement,
    url: string,
): void => {
    iframe.src = url;
};

export const subscribeComponentActionEvent = (
    iframe: HTMLIFrameElement,
    dotnetRef: DotNet.DotNetObject,
    callbackMethodName: string,
): { dispose: () => void } => {
    const handler = (event: MessageEvent) => {
        if (event.origin !== location.origin) return;

        const message = event.data as {
            action?: string;
            frameId?: string;
            name?: string;
            argsJson?: string;
        };

        if (message?.action !== "component-action") return;
        if (message.frameId && message.frameId !== iframe.id) return;
        if (!message.name) return;

        dotnetRef.invokeMethodAsync(
            callbackMethodName,
            message.name,
            message.argsJson ?? "",
        );
    };

    window.addEventListener("message", handler);

    return {
        dispose: () => {
            window.removeEventListener("message", handler);
        },
    };
};

// Checks the '_dotnet_watch_ws_injected' property has been added to the window object,
// indicating that the '_framework/aspnetcore-browser-refresh.js' script has been loaded.
// see: https://github.com/dotnet/sdk/blob/12c083fc90700d3255cc021b665764876c5747fe/src/BuiltInTools/BrowserRefresh/WebSocketScriptInjection.js#L4
const isDotnetWatchScriptInjected = (window: Window | null): boolean => {
    const scriptInjectedSentinel = "_dotnet_watch_ws_injected";
    return window?.hasOwnProperty(scriptInjectedSentinel) ?? false;
};

export const ensureDotnetWatchScriptInjected = async (
    container: HTMLElement,
): Promise<void> => {
    const result = await getIFrame(container);
    if (!result) return;
    const { contentWindow, contentDocument } = result;

    if (!isDotnetWatchScriptInjected(window)) return; // Hot reloading is not available
    if (isDotnetWatchScriptInjected(contentWindow)) return; // Already injected

    const script = contentDocument.createElement("script");
    script.src = "_framework/aspnetcore-browser-refresh.js";
    contentDocument.body.appendChild(script);
};
