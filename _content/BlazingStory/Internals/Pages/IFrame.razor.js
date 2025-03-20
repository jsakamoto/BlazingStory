const keydown = "keydown";
const pointerdown = "pointerdown";
const SessionStateKey = "IFrame.SessionState";
export const initializeCanvasFrame = () => {
    var _a;
    const doc = document;
    const wnd = window;
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
    wnd.BlazingStory = wnd.BlazingStory || {};
    wnd.BlazingStory.canvasFrameInitialized = true;
    const frameElementId = ((_a = wnd.frameElement) === null || _a === void 0 ? void 0 : _a.id) || '';
    const htmlElement = document.body.parentElement;
    const scrollHeight = (htmlElement === null || htmlElement === void 0 ? void 0 : htmlElement.scrollHeight) || 0;
    wnd.parent.postMessage({
        action: "frameview-height",
        frameId: frameElementId,
        height: scrollHeight
    }, location.origin);
    setTimeout(() => htmlElement === null || htmlElement === void 0 ? void 0 : htmlElement.classList.add("_blazing_story_ready_for_visible"), 300);
};
