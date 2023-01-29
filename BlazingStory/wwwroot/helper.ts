import { MessageArgument } from "../Scripts/types";

export const setLocalStorageItem = (key: string, value: string): void => { localStorage.setItem(key, value); };

export const getLocalStorageItem = (key: string): string | null => (localStorage.getItem(key) || null);

export const copyTextToClipboard = (text: string): Promise<void> => navigator.clipboard.writeText(text);

const keydown = "keydown";
const click = "click";

export const setupKeyDownReceiver = (): void => {
    window.addEventListener("message", (event) => {
        if (event.origin !== location.origin) return;

        const message = event.data as MessageArgument;
        switch (message.action) {
            case keydown:
                const keydownEvent = new KeyboardEvent(keydown, { ...message.eventArgs, ...{ bubbles: true } });
                document.body.dispatchEvent(keydownEvent);
                break;
            case click:
                const clickEvent = new MouseEvent(click, { bubbles: true });
                document.body.dispatchEvent(clickEvent);
                break;
        }
    }, false);
}