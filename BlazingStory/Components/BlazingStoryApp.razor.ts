import { DotNetObjectReference } from "../Scripts/types";

export const waitForAllStyleAndFontsAreLoaded = (): Promise<void> => new Promise<void>((resolve) => {

    if (location.pathname !== "/") { resolve(); return; }

    const timerId = setInterval(() => {

        // Wait for all style sheets are loaded.
        const styleSheets = Array.from<HTMLLinkElement>(document.head.querySelectorAll('link[rel="stylesheet"]'));
        if (!styleSheets.every(l => Boolean(l.sheet))) return;

        // Wait for all custom web fonst are loaded.
        const fonts = Array.from(document.fonts);
        if (!fonts.some(f => f.family === "Nunito Sans" && f.weight === "400" && f.status === "unloaded")) return;
        if (!fonts.some(f => f.family === "Nunito Sans" && f.weight === "700" && f.status === "unloaded")) return;

        clearInterval(timerId);
        resolve();
    }, 10);
});

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