export const ensurePreviewStyle = (styleDescripters) => {
    for (const descripter of styleDescripters) {
        const linkElement = document.head.querySelector(`link#${descripter.id}`);
        if (linkElement === null && descripter.enable) {
            const newLinkElement = document.createElement("link");
            newLinkElement.id = descripter.id;
            newLinkElement.href = descripter.href;
            newLinkElement.rel = "stylesheet";
            document.head.appendChild(newLinkElement);
        }
        else if (linkElement !== null && !descripter.enable) {
            linkElement.remove();
        }
    }
};
export const ensureMeasure = (enableMeasure) => {
    const canvasId = "blazingstory-addon-measure";
    const existCanvas = document.body.querySelector("canvas#" + canvasId);
    if (existCanvas !== null && !enableMeasure) {
        existCanvas.remove();
    }
    else if (existCanvas === null && enableMeasure) {
        const canvas = document.createElement("canvas");
        canvas.id = canvasId;
        canvas.style.position = "fixed";
        canvas.style.top = "0";
        canvas.style.left = "0";
        canvas.style.width = "100vw";
        canvas.style.height = "100vh";
        canvas.style.zIndex = "1";
        canvas.style.pointerEvents = "none";
        document.body.appendChild(canvas);
    }
};
