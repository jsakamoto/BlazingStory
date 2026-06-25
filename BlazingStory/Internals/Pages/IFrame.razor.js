const keydown = "keydown";
const pointerdown = "pointerdown";
const SessionStateKey = "IFrame.SessionState";
export const initializeCanvasFrame = () => {
    const wnd = window;
    const doc = document;
    const body = doc.body;
    const htmlElement = body.parentElement;
    body.style.margin = "var(--bs-preview-body-margin, 16px)";
    if (body.style.minHeight === "100vh")
        body.style.removeProperty("min-height");
    const getParentFrame = () => [...wnd.parent.document.querySelectorAll('iframe')].find(f => f.contentWindow === wnd);
    const updateParentFrameType = () => {
        const parentFrame = getParentFrame();
        if (parentFrame?.closest(".docs-page"))
            body.dataset.bsParentFrame = "docs";
        else if (parentFrame?.closest(".canvas-container"))
            body.dataset.bsParentFrame = "story";
        else
            body.dataset.bsParentFrame = "unknown";
    };
    const restoreSessionState = () => {
        const sessionState = {
            ...{ zoom: 1 }, ...JSON.parse(sessionStorage.getItem(SessionStateKey) || "{}")
        };
        body.style.zoom = "var(--bs-zoom, 1)";
        body.style.setProperty("--bs-zoom", "" + sessionState.zoom);
        return sessionState;
    };
    const reset = () => {
        updateParentFrameType();
        return restoreSessionState();
    };
    doc.addEventListener("bs:poolediframe:attached", reset);
    const sessionState = reset();
    wnd.addEventListener("message", (event) => {
        const message = event.data;
        if (event.origin !== location.origin)
            return;
        switch (message.action) {
            case "reload":
                location.reload();
                break;
            case "zoom":
                body.style.setProperty('--bs-zoom', '' + message.zoomLevel);
                body.style.zoom = 'var(--bs-zoom, 1)';
                sessionState.zoom = "" + message.zoomLevel;
                sessionStorage.setItem(SessionStateKey, JSON.stringify(sessionState));
                break;
        }
    }, false);
    doc.addEventListener(keydown, event => {
        const targetElement = event.target;
        if (['INPUT', 'TEXTAREA', 'SELECT'].includes(targetElement.tagName))
            return;
        if (targetElement.contentEditable === "true")
            return;
        wnd.parent.postMessage({
            action: keydown,
            eventArgs: {
                key: event.key,
                code: event.code,
                altKey: event.altKey,
                shiftKey: event.shiftKey,
                ctrlKey: event.ctrlKey,
                metaKey: event.metaKey,
            }
        }, location.origin);
    });
    doc.addEventListener(pointerdown, event => {
        wnd.parent.postMessage({
            action: pointerdown
        }, location.origin);
    });
    if (htmlElement) {
        const measureHeight = () => {
            const style = wnd.getComputedStyle(body);
            const marginTop = parseFloat(style.marginTop) || 0;
            const marginBottom = parseFloat(style.marginBottom) || 0;
            const zoom = parseFloat(style.getPropertyValue('--bs-zoom')) || 1;
            return Math.ceil((body.scrollHeight + marginTop + marginBottom) * zoom);
        };
        const resizeObserver = new ResizeObserver(() => {
            const iframeElement = getParentFrame();
            if (iframeElement) {
                const event = new CustomEvent('frameheightchange', {
                    cancelable: false,
                    bubbles: true,
                    detail: { height: measureHeight() }
                });
                iframeElement.dispatchEvent(event);
            }
        });
        resizeObserver.observe(body);
    }
    if (wnd.BlazingStory)
        wnd.BlazingStory.canvasFrameInitialized = true;
    setTimeout(() => htmlElement?.classList.add("_blazing_story_ready_for_visible"), 300);
};
