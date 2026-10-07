import type { MessageArgument } from "@blazingstory/types/custom-messages";

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
    const wnd = window;
    const doc = document;
    const body = doc.body;
    const htmlElement = body.parentElement;

    // Fix the body style.
    body.style.margin = "var(--bs-preview-body-margin, 16px)";
    if (body.style.minHeight === "100vh") body.style.removeProperty("min-height");

    // Define a function to get the parent frame element (iframe) of the current window.
    const getParentFrame = () => [...wnd.parent.document.querySelectorAll('iframe')].find(f => f.contentWindow === wnd);

    // Define a function to update the parent frame type (docs, story, or unknown) in the data attribute of the body element.
    const updateParentFrameType = () => {
        const parentFrame = getParentFrame();
        if (parentFrame?.closest(".docs-page")) body.dataset.bsParentFrame = "docs";
        else if (parentFrame?.closest(".canvas-container")) body.dataset.bsParentFrame = "story";
        else body.dataset.bsParentFrame = "unknown";
    }

    // Define a function to restore the session state (e.g., zoom level) from the session storage and apply it to the body style.
    const restoreSessionState = () => {
        const sessionState = {
            ...{ zoom: 1 }, ...JSON.parse(sessionStorage.getItem(SessionStateKey) || "{}")
        } as IFrameSessionState;
        body.style.zoom = "var(--bs-zoom, 1)";
        body.style.setProperty("--bs-zoom", "" + sessionState.zoom);
        return sessionState;
    }

    // Define a function to reset the canvas frame, which updates the parent frame type and restores the session state.
    const reset = () => {
        updateParentFrameType();
        return restoreSessionState();
    }

    // Listen to the "bs:poolediframe:attached" event, which is fired when the canvas frame is attached to the parent frame, and reset the canvas frame.
    doc.addEventListener("bs:poolediframe:attached", reset);
    const sessionState = reset();

    // Handle "Reload" message
    wnd.addEventListener("message", (event) => {
        const message = event.data as MessageArgument;
        if (event.origin !== location.origin) return;

        switch (message.action) {
            case "reload":
                location.reload();
                break;
            case "zoom":
                body.style.setProperty('--bs-zoom', '' + message.zoomLevel);
                body.style.zoom = 'var(--bs-zoom, 1)';

                // Save the zoom level to the session storage so that it can be restored when the canvas frame is reloaded or reattached.
                sessionState.zoom = "" + message.zoomLevel;
                sessionStorage.setItem(SessionStateKey, JSON.stringify(sessionState));
                break;
        }
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

    if (htmlElement) {
        // Measure body.scrollHeight, not <html>'s box: scroll-lock implementations that set
        // body.position = 'fixed' collapse <html> to 0, but body.scrollHeight is unaffected.
        //
        // This frame's own "viewport" is the outer <iframe>, whose height is set by the host from
        // the value this function returns (see PreviewFrame.razor.ts / StoryPreview.razor). A host
        // stylesheet that ties <html> or <body> to that viewport - `height: 100%`, `min-height:
        // 100vh`, etc., on either element, however it gets there - turns that into a feedback loop:
        // each measurement includes the height just assigned by the *previous* one, so the reported
        // height (plus the body margin added below) grows every cycle instead of settling.
        // Chasing every such rule (inline or stylesheet, vh or %) is a losing game, so instead
        // neutralize both elements for the instant of the measurement - a synchronous read with no
        // intervening layout/paint, so there is no visible flicker - which makes scrollHeight reflect
        // only the natural content height regardless of what the host's CSS ties to the viewport.
        const captureProperty = (element: HTMLElement, property: string) => ({
            value: element.style.getPropertyValue(property),
            priority: element.style.getPropertyPriority(property)
        });

        const restoreProperty = (element: HTMLElement, property: string, saved: { value: string, priority: string }) => {
            if (saved.value) element.style.setProperty(property, saved.value, saved.priority);
            else element.style.removeProperty(property);
        };

        const measureHeight = () => {
            const savedHtmlHeight = captureProperty(htmlElement, "height");
            const savedHtmlMinHeight = captureProperty(htmlElement, "min-height");
            const savedBodyHeight = captureProperty(body, "height");
            const savedBodyMinHeight = captureProperty(body, "min-height");
            htmlElement.style.setProperty("height", "auto", "important");
            htmlElement.style.setProperty("min-height", "0", "important");
            body.style.setProperty("height", "auto", "important");
            body.style.setProperty("min-height", "0", "important");

            try {
                const style = wnd.getComputedStyle(body);
                const marginTop = parseFloat(style.marginTop) || 0;
                const marginBottom = parseFloat(style.marginBottom) || 0;
                const zoom = parseFloat(style.getPropertyValue('--bs-zoom')) || 1;
                return Math.ceil((body.scrollHeight + marginTop + marginBottom) * zoom);
            } finally {
                restoreProperty(htmlElement, "height", savedHtmlHeight);
                restoreProperty(htmlElement, "min-height", savedHtmlMinHeight);
                restoreProperty(body, "height", savedBodyHeight);
                restoreProperty(body, "min-height", savedBodyMinHeight);
            }
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

    if (wnd.BlazingStory) wnd.BlazingStory.canvasFrameInitialized = true;

    // After initialization, add a class to the html element to make the frame scrollable.
    // (The html element without the "_blazing_story_ready_for_visible" CSS class is applied "overflow:none")
    // This is required to make annoying scroll bars invisible while adjusting the preview frame size to fit iframe contents.
    // After adjustment, the CSS class is added, and then the preview frame contents are scrollable.
    // (See also: BlazingStory/Components/BlazingStoryApp.razor)
    setTimeout(() => htmlElement?.classList.add("_blazing_story_ready_for_visible"), 300);
}