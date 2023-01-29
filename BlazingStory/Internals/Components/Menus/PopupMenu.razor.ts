import { DotNetObjectReference, IDisposable } from "../../../Scripts/types";

export const getBoundingClientRect = (element:HTMLElement) => {
    return element.getBoundingClientRect();
};

export const subscribeDocumentEvent = (eventType: string, dotnetObj: DotNetObjectReference, methodName: string, excludeSelector: string): IDisposable => {

    const evendListener = (ev: Event) => {
        if (excludeSelector && ev.target && (ev.target as HTMLElement).matches(excludeSelector)) return;
        dotnetObj.invokeMethodAsync(methodName);
    }
    document.addEventListener(eventType, evendListener);
    return ({ dispose: () => document.removeEventListener(eventType, evendListener) });
}