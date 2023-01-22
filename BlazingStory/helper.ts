export const setLocalStorageItem = (key: string, value: string): void => { localStorage.setItem(key, value); };

export const getLocalStorageItem = (key: string): string | null => (localStorage.getItem(key) || null);

export const copyTextToClipboard = (text: string): Promise<void> => navigator.clipboard.writeText(text);

const keydown = "keydown";

type KeyEventArgument = {
    key: string,
    code: string,
    altKey: boolean,
    shiftKey: boolean,
    ctrlKey: boolean,
    metaKey: boolean,
};

type MessageArgument = {
    action: "keydown",
    eventArgs: KeyEventArgument
}

export const setupKeyDownSender = (): void => {
    document.addEventListener(keydown, event => {
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
        }, location.origin);
    });
}

export const setupKeyDownReceiver = (): void => {
    window.addEventListener("message", (event) => {
        if (event.origin !== location.origin) return;
        const message = event.data as MessageArgument;
        if (message.action !== keydown) return;
        const keydownEvent = new KeyboardEvent(keydown, { ...message.eventArgs, ...{ bubbles: true } });
        document.body.dispatchEvent(keydownEvent);
    }, false);
}