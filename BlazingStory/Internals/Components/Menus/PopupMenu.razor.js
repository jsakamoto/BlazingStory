export const subscribeDocumentEvent = (eventType, dotnetObj, methodName, popupMenuElement) => {
    const triggerContent = popupMenuElement.querySelector('.popup-menu-trigger-content');
    const evendListener = (ev) => {
        if (triggerContent && triggerContent.contains(ev.target))
            return;
        dotnetObj.invokeMethodAsync(methodName);
    };
    document.addEventListener(eventType, evendListener);
    return ({ dispose: () => document.removeEventListener(eventType, evendListener) });
};
