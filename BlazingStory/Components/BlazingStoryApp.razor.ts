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
