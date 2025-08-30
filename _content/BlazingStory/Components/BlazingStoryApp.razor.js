const delay = (ms) => new Promise(resolve => setTimeout(resolve, ms));
export const ensureAllFontsAndStylesAreLoaded = async () => {
    if (location.pathname !== "/")
        return;
    const startTime = Date.now();
    await Promise.allSettled([...document.fonts].map(font => font.load()));
    const isAllFontsLoaded = () => [...document.fonts].every(font => font.status === "loaded");
    const isAllStylesheetsLoaded = () => [...document.head.querySelectorAll('link[rel="stylesheet"]')].every(link => link.sheet);
    while (!isAllFontsLoaded() || !isAllStylesheetsLoaded()) {
        await delay(10);
        if (Date.now() - startTime > 5000) {
            console.warn("Timeout waiting for fonts and stylesheets to load");
            break;
        }
    }
};
const darkModeMediaQuery = matchMedia("(prefers-color-scheme: dark)");
export const getPrefersColorScheme = () => darkModeMediaQuery.matches ? "dark" : "light";
export const subscribePreferesColorSchemeChanged = (dotnetObjRef, methodName) => {
    const handler = () => void dotnetObjRef.invokeMethodAsync(methodName, getPrefersColorScheme());
    darkModeMediaQuery.addEventListener("change", handler);
    return { dispose: () => darkModeMediaQuery.removeEventListener("change", handler) };
};
