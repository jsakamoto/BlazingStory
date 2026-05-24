const keydown = "keydown";
const pointerdown = "pointerdown";
const SessionStateKey = "IFrame.SessionState";
const getParentFrame = (wnd) => {
    return [...wnd.parent.document.querySelectorAll('iframe')].find(f => f.contentWindow === wnd);
};
const updateParentFrameType = (wnd, body) => {
    const parentFrame = getParentFrame(wnd);
    if (parentFrame?.closest(".docs-page"))
        body.dataset.bsParentFrame = "docs";
    else if (parentFrame?.closest(".canvas-container"))
        body.dataset.bsParentFrame = "story";
    else
        body.dataset.bsParentFrame = "unknown";
};
export const initializeCanvasFrame = () => {
    const wnd = window;
    const doc = document;
    const body = doc.body;
    const htmlElement = body.parentElement;
    body.style.margin = "var(--bs-preview-body-margin, 16px)";
    if (body.style.minHeight === "100vh")
        body.style.removeProperty("min-height");
    doc.addEventListener("bs:poolediframe:attached", () => updateParentFrameType(wnd, body));
    updateParentFrameType(wnd, body);
    const sessionState = {
        ...{ zoom: 1 }, ...JSON.parse(sessionStorage.getItem(SessionStateKey) || "{}")
    };
    body.style.zoom = "var(--bs-zoom, 1)";
    body.style.setProperty("--bs-zoom", "" + sessionState.zoom);
    wnd.addEventListener("message", (event) => {
        const message = event.data;
        if (event.origin !== location.origin || message.action !== "reload")
            return;
        const computedStyle = window.getComputedStyle(body);
        sessionState.zoom = "" + parseFloat(computedStyle.getPropertyValue('--bs-zoom') || computedStyle.zoom || '1');
        sessionStorage.setItem(SessionStateKey, JSON.stringify(sessionState));
        location.reload();
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
        const resizeObserver = new ResizeObserver((entries) => {
            for (const entry of entries) {
                const height = Math.ceil(entry.target.getBoundingClientRect().height);
                const iframeElement = getParentFrame(wnd);
                if (iframeElement) {
                    const event = new CustomEvent('frameheightchange', {
                        cancelable: false,
                        bubbles: true,
                        detail: { height }
                    });
                    iframeElement.dispatchEvent(event);
                }
            }
        });
        resizeObserver.observe(htmlElement);
    }
    wnd.BlazingStory = wnd.BlazingStory || {};
    wnd.BlazingStory.canvasFrameInitialized = true;
    setTimeout(() => htmlElement?.classList.add("_blazing_story_ready_for_visible"), 300);
};
