export const ensureAllFontsAndStylesAreLoaded = async () => {
    if (location.pathname !== "/")
        return;
    const delay = (ms) => new Promise(resolve => setTimeout(resolve, ms));
    for (const font of document.fonts)
        font.load();
    for (;;) {
        const fonts = Array.from(document.fonts);
        if (fonts.every(font => font.status === "loaded"))
            break;
        await delay(10);
    }
    for (;;) {
        const styleSheets = Array.from(document.head.querySelectorAll('link[rel="stylesheet"]'));
        if (styleSheets.every(l => Boolean(l.sheet)))
            break;
        await delay(10);
    }
};
const darkModeMediaQuery = window.matchMedia("(prefers-color-scheme: dark)");
export const getPrefersColorScheme = () => darkModeMediaQuery.matches ? "dark" : "light";
export const subscribePreferesColorSchemeChanged = (dotnetObjRef, methodName) => {
    const subscriber = (e) => { dotnetObjRef.invokeMethodAsync(methodName, getPrefersColorScheme()); };
    darkModeMediaQuery.addEventListener("change", subscriber);
    return ({ dispose: () => darkModeMediaQuery.removeEventListener("change", subscriber) });
};
