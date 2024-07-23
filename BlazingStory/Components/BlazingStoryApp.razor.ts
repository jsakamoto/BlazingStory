import { DotNetObjectReference, IDisposable } from '../Scripts/types';

export const ensureAllFontsAndStylesAreLoaded = async () => {
    // Check if not on the homepage, then return early
    if (location.pathname !== '/') return;

    const delay = (ms: number) =>
        new Promise((resolve) => setTimeout(resolve, ms));

    // Ensure all fonts are loaded
    const fonts = Array.from((document as any).fonts) as FontFace[];
    await Promise.all(fonts.map((font) => font.load()));

    // Wait until all fonts are loaded
    for (; ;) {
        if (fonts.every((font) => font.status === 'loaded')) {
            break;
        }
        await delay(10); // Wait 10 milliseconds before checking again
    }

    // Wait for all style sheets to be loaded
    for (; ;) {
        const styleSheets = Array.from<HTMLLinkElement>(
            document.head.querySelectorAll('link[rel="stylesheet"]'),
        );
        if (styleSheets.every((l) => Boolean(l.sheet))) {
            break;
        }
        await delay(10); // Wait 10 milliseconds before checking again
    }
};

const darkModeMediaQuery = window.matchMedia('(prefers-color-scheme: dark)');

export const getPrefersColorScheme = (): string =>
    darkModeMediaQuery.matches ? 'dark' : 'light';

export const subscribePreferesColorSchemeChanged = (
    dotnetObjRef: DotNetObjectReference,
    methodName: string,
): IDisposable => {
    const subscriber = (e: MediaQueryListEvent) => {
        dotnetObjRef.invokeMethodAsync(methodName, getPrefersColorScheme());
    };
    darkModeMediaQuery.addEventListener('change', subscriber);
    return {
        dispose: () => darkModeMediaQuery.removeEventListener('change', subscriber),
    };
};
