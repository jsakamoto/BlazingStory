import { DotNetObjectReference, IDisposable } from "../Scripts/types";

const delay = (ms: number) => new Promise<void>(resolve => setTimeout(resolve, ms));

/**
 * Ensures all fonts and stylesheets are fully loaded before proceeding
 * Only executes on the root path to optimize performance
 */
export const ensureAllFontsAndStylesAreLoaded = async (): Promise<void> => {
    if (location.pathname !== "/") return;

    // Initiate font loading concurrently
    await Promise.allSettled([...document.fonts].map(font => font.load()));

    // Wait for fonts with efficient polling and fail-safe timeout
    await Promise.race([
        new Promise<void>(resolve => {
            const checkFonts = (): void => 
                [...document.fonts].every(font => font.status === "loaded") 
                    ? resolve() 
                    : setTimeout(checkFonts, 10);
            checkFonts();
        }),
        delay(5000)
    ]);

    // Wait for stylesheets with efficient polling and fail-safe timeout
    await Promise.race([
        new Promise<void>(resolve => {
            const checkStyles = (): void => 
                [...document.head.querySelectorAll<HTMLLinkElement>('link[rel="stylesheet"]')].every(link => link.sheet)
                    ? resolve()
                    : setTimeout(checkStyles, 10);
            checkStyles();
        }),
        delay(5000)
    ]);
};

const darkModeMediaQuery = matchMedia("(prefers-color-scheme: dark)");

/**
 * Returns current color scheme preference
 */
export const getPrefersColorScheme = (): "dark" | "light" => 
    darkModeMediaQuery.matches ? "dark" : "light";

/**
 * Subscribes to color scheme changes with automatic cleanup
 */
export const subscribePreferesColorSchemeChanged = (
    dotnetObjRef: DotNetObjectReference, 
    methodName: string
): IDisposable => {
    const handler = (): void => dotnetObjRef.invokeMethodAsync(methodName, getPrefersColorScheme());
    
    darkModeMediaQuery.addEventListener("change", handler);
    return { dispose: () => darkModeMediaQuery.removeEventListener("change", handler) };
};
