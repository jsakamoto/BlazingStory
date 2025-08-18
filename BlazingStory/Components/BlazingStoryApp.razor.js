const delay = (ms) => new Promise(resolve => setTimeout(resolve, ms));
export const ensureAllFontsAndStylesAreLoaded = async () => {
    if (location.pathname !== "/")
        return;
    await Promise.allSettled([...document.fonts].map(font => font.load()));
    await Promise.race([
        new Promise(resolve => {
            const checkFonts = () => [...document.fonts].every(font => font.status === "loaded")
                ? resolve()
                : void setTimeout(checkFonts, 10);
            checkFonts();
        }),
        delay(5000)
    ]);
    await Promise.race([
        new Promise(resolve => {
            const checkStyles = () => [...document.head.querySelectorAll('link[rel="stylesheet"]')].every(link => link.sheet)
                ? resolve()
                : void setTimeout(checkStyles, 10);
            checkStyles();
        }),
        delay(5000)
    ]);
};
const darkModeMediaQuery = matchMedia("(prefers-color-scheme: dark)");
export const getPrefersColorScheme = () => darkModeMediaQuery.matches ? "dark" : "light";
export const subscribePreferesColorSchemeChanged = (dotnetObjRef, methodName) => {
    const handler = () => void dotnetObjRef.invokeMethodAsync(methodName, getPrefersColorScheme());
    darkModeMediaQuery.addEventListener("change", handler);
    return { dispose: () => darkModeMediaQuery.removeEventListener("change", handler) };
};
