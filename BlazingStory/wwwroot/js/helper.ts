import { MessageArgument } from "../Scripts/types";

const doc = document;
const lstorage = localStorage;

export const setLocalStorageItem = (key: string, value: string): void => { lstorage.setItem(key, value); };

export const getLocalStorageItem = (key: string): string | null => (lstorage.getItem(key) || null);

export const copyTextToClipboard = (text: string): Promise<void> => navigator.clipboard.writeText(text);

export const releaseFocus = (): void => { (doc.activeElement as HTMLElement)?.blur(); };

const keydown = "keydown";
const pointerdown = "pointerdown";

/**
 * Set up a message receiver from iframe elements.
 */
export const setupMessageReceiverFromIFrame = (): void => {

    window.addEventListener("message", (event) => {
        if (event.origin !== location.origin) return;

        const message = event.data as MessageArgument;

        switch (message.action) {

            // Transfer keydown event that occurred inside of iframe elements to this document.
            // This is required to make the Blazing Story's hotkeys work, even when an element inside an iframe has focus.
            // (See also: BlazingStory/Internals/Pages/IFrame.razor.ts)
            case keydown:
                const keydownEvent = new KeyboardEvent(keydown, { ...message.eventArgs, ...{ bubbles: true } });
                doc.body.dispatchEvent(keydownEvent);
                break;

            // Transfer pointerdown event that occurred inside of iframe elements to this document.
            // This is required to ensure popup menus are closed, even when a user clicks inside an iframe.
            // (See also: BlazingStory/Internals/Pages/IFrame.razor.ts)
            case pointerdown:
                const clickEvent = new MouseEvent(pointerdown, { bubbles: true });
                doc.body.dispatchEvent(clickEvent);
                break;

            // Set the height of the preview frame element to be the height reported from the iframe.
            // This is required to make a vertical scroll bar never shown in preview frames on the "Docs" page.
            // (See also: BlazingStory/Internals/Pages/IFrame.razor.ts)
            case "frameview-height":
                const previewFrameViewPort = doc.querySelector(`.preview-frame-viewport:has(iframe#${message.frameId})`) as HTMLElement | null;
                if (previewFrameViewPort) {
                    previewFrameViewPort.style.height = message.height + "px";
                }
                break;
        }
    }, false);
}