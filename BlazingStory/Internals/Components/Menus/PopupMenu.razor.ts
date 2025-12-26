import type { DotNetObjectReference, IDisposable } from "../../../Scripts/types";

export const subscribeDocumentEvent = (eventType: string, dotnetObj: DotNetObjectReference, methodName: string, popupMenuElement: HTMLElement): IDisposable => {

    const evendListener = (ev: Event) => {
        if (popupMenuElement.contains(ev.target as HTMLElement)) return;
        dotnetObj.invokeMethodAsync(methodName);
    }
    document.addEventListener(eventType, evendListener);
    return ({ dispose: () => document.removeEventListener(eventType, evendListener) });
}