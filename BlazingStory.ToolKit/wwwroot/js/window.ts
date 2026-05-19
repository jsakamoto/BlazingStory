import type { IDisposable } from "@blazingstory/types/disposable";
import type { DotNetObjectReference } from "@blazingstory/types/blazor";

const getTargetWindow = async (selector: string): Promise<Window> => {
    if (selector === "parent") return window.parent;
    for (let i = 0; i < 30; i++) {
        const iframe = document.querySelector(selector);
        if (iframe instanceof HTMLIFrameElement && iframe.contentWindow) return iframe.contentWindow;
        await new Promise(resolve => setTimeout(resolve, 100));
    }
    throw new Error(`No target window found for selector: ${selector}`);
}

export const postMessage = async (selector: string, message?: string) => {
    const targetWindow = await getTargetWindow(selector);
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