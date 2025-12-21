import { CSSStyle, MessageArgument } from "../../Scripts/types";

const keydown = "keydown";
const pointerdown = "pointerdown";
const SessionStateKey = "IFrame.SessionState";

type IFrameSessionState = {
    zoom: string
}

/**
 * Initialize the canvas (preview) frame.
 */
export const initializeCanvasFrame = () => {
    const doc = document;
    const wnd = window;

    // Restore the session state.
    const sessionState = {
        ...{ zoom: 1 }, ...JSON.parse(sessionStorage.getItem(SessionStateKey) || "{}")
    } as IFrameSessionState;
    (doc.body.style as CSSStyle).zoom = "" + sessionState.zoom;

    // Handle "Reload" message
    wnd.addEventListener("message", (event) => {
        const message = event.data as MessageArgument;
        if (event.origin !== location.origin || message.action !== "reload") return;

        // Save state to session storage before reloading.
        sessionState.zoom = (doc.body.style as CSSStyle).zoom || "1";
        sessionStorage.setItem(SessionStateKey, JSON.stringify(sessionState));

        location.reload();
    }, false);

    // Transfer the keydown event to parent window.
    // This is required to make the Blazing Story's hotkeys work, even when an element inside an iframe has focus.
    // (See also: BlazingStory/wwwroot/helper.ts)
    doc.addEventListener(keydown, event => {

        // Do not transfer the keydown event to the BlazingStory app when the target element is an input or editable element.
        // (Otherwise, keyboard shortcuts will be fired when the user is typing in the input element.)
        const targetElement = event.target as HTMLElement;
        if (['INPUT', 'TEXTAREA', 'SELECT'].includes(targetElement.tagName)) return;
        if (targetElement.contentEditable === "true") return;

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
        } as MessageArgument, location.origin);
    });

    // Transfer the click event to parent window.
    // This is required to ensure popup menus are closed, even when a user clicks inside an iframe.
    // (See also: BlazingStory/wwwroot/helper.ts)
    doc.addEventListener(pointerdown, event => {
        wnd.parent.postMessage({
            action: pointerdown
        } as MessageArgument, location.origin);
    });

    wnd.BlazingStory = wnd.BlazingStory || {};
    wnd.BlazingStory.canvasFrameInitialized = true;

    // After initialization, add a class to the html element to make the frame scrollable.
    // (The html element without the "_blazing_story_ready_for_visible" CSS class is applied "overflow:none")
    // This is required to make annoying scroll bars invisible while adjusting the preview frame size to fit iframe contents.
    // After adjustment, the CSS class is added, and then the preview frame contents are scrollable.
    // (See also: BlazingStory/Components/BlazingStoryApp.razor)
    const htmlElement = doc.body.parentElement;
    setTimeout(() => htmlElement?.classList.add("_blazing_story_ready_for_visible"), 300);
}