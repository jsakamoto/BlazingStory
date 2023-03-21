export const waitForAllStyleAndFontsAreLoaded = () => new Promise((resolve) => {
    if (location.pathname !== "/") {
        resolve();
        return;
    }
    const timerId = setInterval(() => {
        const styleSheets = Array.from(document.head.querySelectorAll('link[rel="stylesheet"]'));
        if (!styleSheets.every(l => Boolean(l.sheet)))
            return;
        const fonts = Array.from(document.fonts);
        if (!fonts.some(f => f.family === "Nunito Sans" && f.weight === "400" && f.status === "unloaded"))
            return;
        if (!fonts.some(f => f.family === "Nunito Sans" && f.weight === "700" && f.status === "unloaded"))
            return;
        clearInterval(timerId);
        resolve();
    }, 10);
});
