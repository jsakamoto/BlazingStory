import { CSSStyle, MessageArgument } from "../../Scripts/types";

const keydown = "keydown";
const pointerdown = "pointerdown";
const SessionStateKey = "IFrame.SessionState";

type IFrameSessionState = {
    zoom: string
}

export const initializeCanvasFrame = () => {

    // Restore the session state.
    const sessionState = {
        ...{ zoom: 1 }, ...JSON.parse(sessionStorage.getItem(SessionStateKey) || "{}")
    } as IFrameSessionState;
    (document.body.style as CSSStyle).zoom = "" + sessionState.zoom;

    // Handle "Reload" message
    window.addEventListener("message", (event) => {
        const message = event.data as MessageArgument;
        if (event.origin !== location.origin || message.action !== "reload") return;

        // Save state to session storage before reloading.
        sessionState.zoom = (document.body.style as CSSStyle).zoom || "1";
        sessionStorage.setItem(SessionStateKey, JSON.stringify(sessionState));

        location.reload();
    }, false);

    // Transfer the keydown event to parent window.
    document.addEventListener(keydown, event => {

        // Do not transfer the keydown event to the BlazingStory app when the target element is an input or editable element.
        // (Otherwise, keyboard shortcuts will be fired when the user is typing in the input element.)
        const targetElement = event.target as HTMLElement;
        if (['INPUT', 'TEXTAREA', 'SELECT'].includes(targetElement.tagName)) return;
        if (targetElement.contentEditable === "true") return;

        window.parent.postMessage({
            action: keydown,
            eventArgs: {
                key: event.key,
                code: event.code,
                altKey: event.altKey,
                shiftKey: event.shiftKey,
                ctrlKey: event.ctrlKey,
                metaKey: event.metaKey,
            }
        } as MessageArgument, location.origin);
    });

    // Transfer the click event to parent window.
    document.addEventListener(pointerdown, event => {
        window.parent.postMessage({
            action: pointerdown
        } as MessageArgument, location.origin);
    });

    window.BlazingStory = window.BlazingStory || {};
    window.BlazingStory.canvasFrameInitialized = true;
}