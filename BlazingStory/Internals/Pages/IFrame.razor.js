import { CSSStyle, MessageArgument } from "../../Scripts/types";
const keydown = "keydown";
const pointerdown = "pointerdown";
const SessionStateKey = "IFrame.SessionState";
export const initializeCanvasFrame = () => {
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
    const htmlElement = doc.body.parentElement;
    setTimeout(() => htmlElement?.classList.add("_blazing_story_ready_for_visible"), 300);
};
