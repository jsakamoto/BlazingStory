import type { IDisposable } from "../wwwroot/js/types/disposable";
import type { DotNetObjectReference } from "../wwwroot/js/types/blazor";

const delay = (ms: number) => new Promise<void>(resolve => setTimeout(resolve, ms));

/**
 * Ensures all fonts and stylesheets are fully loaded before proceeding
 * Only executes on the root path to optimize performance
 */
export const ensureAllFontsAndStylesAreLoaded = async (): Promise<void> => {
    if (location.pathname !== "/") return;

    const startTime = Date.now();

    // Initiate font loading concurrently
    await Promise.allSettled([...document.fonts].map(font => font.load()));

    // Wait until all fonts and stylesheets are loaded
    const isAllFontsLoaded = () => [...document.fonts].every(font => font.status === "loaded");
    const isAllStylesheetsLoaded = () => [...document.head.querySelectorAll<HTMLLinkElement>('link[rel="stylesheet"]')].every(link => link.sheet);
    while (!isAllFontsLoaded() || !isAllStylesheetsLoaded()) {
        await delay(10);

        // Prevent infinite loop by checking elapsed time
        if (Date.now() - startTime > 5000) {
            console.warn("Timeout waiting for fonts and stylesheets to load");
            break;
        }
    }
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
    const handler = (): void => void dotnetObjRef.invokeMethodAsync(methodName, getPrefersColorScheme());

    darkModeMediaQuery.addEventListener("change", handler);
    return { dispose: () => darkModeMediaQuery.removeEventListener("change", handler) };
};
