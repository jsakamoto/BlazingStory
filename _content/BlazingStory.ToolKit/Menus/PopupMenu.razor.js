export const subscribeDocumentEvent = (eventType, dotnetObj, methodName, popupMenuElement) => {
    const evendListener = (ev) => {
        if (popupMenuElement.contains(ev.target))
            return;
        dotnetObj.invokeMethodAsync(methodName);
    };
    document.addEventListener(eventType, evendListener);
    return ({ dispose: () => document.removeEventListener(eventType, evendListener) });
};
