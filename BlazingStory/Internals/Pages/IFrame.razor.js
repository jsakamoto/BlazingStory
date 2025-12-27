const keydown = "keydown";
const pointerdown = "pointerdown";
const SessionStateKey = "IFrame.SessionState";
export const initializeCanvasFrame = () => {
    const doc = document;
    const wnd = window;
    const htmlElement = doc.body.parentElement;
    const sessionState = {
        ...{ zoom: 1 }, ...JSON.parse(sessionStorage.getItem(SessionStateKey) || "{}")
    };
    doc.body.style.zoom = "" + sessionState.zoom;
    wnd.addEventListener("message", (event) => {
        const message = event.data;
        if (event.origin !== location.origin || message.action !== "reload")
            return;
        sessionState.zoom = doc.body.style.zoom || "1";
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
                const iframeElement = [...wnd.parent.document.querySelectorAll('iframe')].find(f => f.contentWindow === wnd);
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
