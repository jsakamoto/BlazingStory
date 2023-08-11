import { DotNetObjectReference } from "../Scripts/types";

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

let subscriberIndex = 0;
const subscribers = new Map<number, (e: MediaQueryListEvent) => void>();

export const subscribePreferesColorSchemeChanged = (dotnetObjRef: DotNetObjectReference, methodName: string): number => {
    const subscriber = (e: MediaQueryListEvent) => {
        dotnetObjRef.invokeMethodAsync(methodName, getPrefersColorScheme());
    };
    darkModeMediaQuery.addEventListener("change", subscriber);

    subscriberIndex++;
    subscribers.set(subscriberIndex, subscriber);
    return subscriberIndex;
}

export const unsubscribePreferesColorSchemeChanged = (subscriberIndex: number): void => {
    const subscriber = subscribers.get(subscriberIndex);
    if (typeof (subscriber) === "undefined") return;
    darkModeMediaQuery.removeEventListener("change", subscriber);
    subscribers.delete(subscriberIndex);
}