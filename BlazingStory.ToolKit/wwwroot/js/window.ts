import type { IDisposable } from "@blazingstory/types/disposable";
import type { DotNetObjectReference } from "@blazingstory/types/blazor";

const getTargetWindow = (selector: string): Window => {
    if (selector === "parent") return window.parent;
    const iframe = document.querySelector(selector);
    if (iframe instanceof HTMLIFrameElement && iframe.contentWindow) return iframe.contentWindow;
    throw new Error(`No target window found for selector: ${selector}`);
}

export const postMessage = (selector: string, message?: string) => {
    const targetWindow = getTargetWindow(selector);
    targetWindow.postMessage(message, location.origin);
};

export const setupMessageListener = (obj: DotNetObjectReference): IDisposable => {

    const handler = (event: MessageEvent) => {
        if (event.origin !== location.origin) return;
        const message = typeof (event.data) === "string" ? event.data : JSON.stringify(event.data);
        obj.invokeMethodAsync("OnMessageReceived", message);
    };

    window.addEventListener("message", handler);

    return {
        dispose: () => {
            window.removeEventListener("message", handler);
        }
    }
}