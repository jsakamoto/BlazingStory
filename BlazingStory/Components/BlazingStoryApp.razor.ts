import { DotNetObjectReference, IDisposable } from "../Scripts/types";

export const ensureAllFontsAndStylesAreLoaded = async () => {

    if (location.pathname !== "/") return;

    const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

    // Ensure all fonts are loaded.
    for (const font of document.fonts) font.load();
    for (; ;) {
        const fonts = Array.from(document.fonts);
        if (fonts.every(font => font.status === "loaded")) break;
        await delay(10);
    }

    // Wait for all style sheets are loaded.
    for (; ;) {
        const styleSheets = Array.from<HTMLLinkElement>(document.head.querySelectorAll('link[rel="stylesheet"]'));
        if (styleSheets.every(l => Boolean(l.sheet))) break;
        await delay(10);
    }
}

const darkModeMediaQuery = window.matchMedia("(prefers-color-scheme: dark)");

export const getPrefersColorScheme = (): string => darkModeMediaQuery.matches ? "dark" : "light";

export const subscribePreferesColorSchemeChanged = (dotnetObjRef: DotNetObjectReference, methodName: string): IDisposable => {
    const subscriber = (e: MediaQueryListEvent) => { dotnetObjRef.invokeMethodAsync(methodName, getPrefersColorScheme()); };
    darkModeMediaQuery.addEventListener("change", subscriber);
    return ({ dispose: () => darkModeMediaQuery.removeEventListener("change", subscriber) });
}