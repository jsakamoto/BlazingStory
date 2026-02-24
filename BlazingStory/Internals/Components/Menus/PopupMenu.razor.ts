import type { IDisposable } from "../../../wwwroot/js/types/disposable";
import type { DotNetObjectReference } from "../../../wwwroot/js/types/blazor";

export const subscribeDocumentEvent = (eventType: string, dotnetObj: DotNetObjectReference, methodName: string, popupMenuElement: HTMLElement): IDisposable => {

    const triggerContent = popupMenuElement.querySelector('.popup-menu-trigger-content');
    const evendListener = (ev: Event) => {
        if (triggerContent && triggerContent.contains(ev.target as HTMLElement)) return;
        dotnetObj.invokeMethodAsync(methodName);
    }
    document.addEventListener(eventType, evendListener);
    return ({ dispose: () => document.removeEventListener(eventType, evendListener) });
}