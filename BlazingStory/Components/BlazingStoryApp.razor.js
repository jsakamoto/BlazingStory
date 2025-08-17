const delay = (ms) => new Promise(resolve => setTimeout(resolve, ms));

/**
 * Ensures all fonts and stylesheets are fully loaded before proceeding
 * Only executes on the root path to optimize performance
 */
export const ensureAllFontsAndStylesAreLoaded = async () => {
    if (location.pathname !== "/") return;

    // Initiate font loading concurrently
    await Promise.allSettled([...document.fonts].map(font => font.load()));

    // Wait for fonts with efficient polling
    await Promise.race([
        new Promise(resolve => {
            const checkFonts = () => 
                [...document.fonts].every(font => font.status === "loaded") 
                    ? resolve() 
                    : setTimeout(checkFonts, 10);
            checkFonts();
        }),
        delay(5000) // Fail-safe timeout
    ]);

    // Wait for stylesheets with efficient polling
    await Promise.race([
        new Promise(resolve => {
            const checkStyles = () => 
                [...document.head.querySelectorAll('link[rel="stylesheet"]')].every(link => link.sheet)
                    ? resolve()
                    : setTimeout(checkStyles, 10);
            checkStyles();
        }),
        delay(5000) // Fail-safe timeout
    ]);
};

const darkModeMediaQuery = matchMedia("(prefers-color-scheme: dark)");

/**
 * Returns current color scheme preference
 */
export const getPrefersColorScheme = () => darkModeMediaQuery.matches ? "dark" : "light";

/**
 * Subscribes to color scheme changes with automatic cleanup
 */
export const subscribePreferesColorSchemeChanged = (dotnetObjRef, methodName) => {
    const handler = () => dotnetObjRef.invokeMethodAsync(methodName, getPrefersColorScheme());
    
    darkModeMediaQuery.addEventListener("change", handler);
    return { dispose: () => darkModeMediaQuery.removeEventListener("change", handler) };
};
