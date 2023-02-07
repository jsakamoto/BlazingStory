export const subscribeDocumentEvent = (eventType, dotnetObj, methodName, excludeSelector) => {
    const evendListener = (ev) => {
        if (excludeSelector && ev.target && ev.target.matches(excludeSelector))
            return;
        dotnetObj.invokeMethodAsync(methodName);
    };
    document.addEventListener(eventType, evendListener);
    return ({ dispose: () => document.removeEventListener(eventType, evendListener) });
};
