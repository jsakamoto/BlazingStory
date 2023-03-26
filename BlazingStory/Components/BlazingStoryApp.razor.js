export const waitForAllStyleAndFontsAreLoaded = () => new Promise((resolve) => {
    if (location.pathname !== "/") {
        resolve();
        return;
    }
    const timerId = setInterval(() => {
        const styleSheets = Array.from(document.head.querySelectorAll('link[rel="stylesheet"]'));
        if (!styleSheets.every(l => Boolean(l.sheet)))
            return;
        const fonts = Array.from(document.fonts);
        if (!fonts.some(f => f.family === "Nunito Sans" && f.weight === "400" && f.status === "unloaded"))
            return;
        if (!fonts.some(f => f.family === "Nunito Sans" && f.weight === "700" && f.status === "unloaded"))
            return;
        clearInterval(timerId);
        resolve();
    }, 10);
});
const darkModeMediaQuery = window.matchMedia("(prefers-color-scheme: dark)");
export const getPrefersColorScheme = () => darkModeMediaQuery.matches ? "dark" : "light";
let subscriberIndex = 0;
const subscribers = new Map();
export const subscribePreferesColorSchemeChanged = (dotnetObjRef, methodName) => {
    const subscriber = (e) => {
        dotnetObjRef.invokeMethodAsync(methodName, getPrefersColorScheme());
    };
    darkModeMediaQuery.addEventListener("change", subscriber);
    subscriberIndex++;
    subscribers.set(subscriberIndex, subscriber);
    return subscriberIndex;
};
export const unsubscribePreferesColorSchemeChanged = (subscriberIndex) => {
    const subscriber = subscribers.get(subscriberIndex);
    if (typeof (subscriber) === "undefined")
        return;
    darkModeMediaQuery.removeEventListener("change", subscriber);
    subscribers.delete(subscriberIndex);
};
