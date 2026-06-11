const doc = document;
const lstorage = localStorage;
export const copyTextToClipboard = (text) => navigator.clipboard.writeText(text);
export const releaseFocus = () => { doc.activeElement?.blur(); };
const keydown = "keydown";
const pointerdown = "pointerdown";
export const setupMessageReceiverFromIFrame = () => {
    window.addEventListener("message", (event) => {
        if (event.origin !== location.origin)
            return;
        const message = event.data;
        switch (message.action) {
            case keydown:
                const keydownEvent = new KeyboardEvent(keydown, { ...message.eventArgs, ...{ bubbles: true } });
                doc.body.dispatchEvent(keydownEvent);
                break;
            case pointerdown:
                const clickEvent = new MouseEvent(pointerdown, { bubbles: true });
                doc.body.dispatchEvent(clickEvent);
                break;
        }
    }, false);
};
