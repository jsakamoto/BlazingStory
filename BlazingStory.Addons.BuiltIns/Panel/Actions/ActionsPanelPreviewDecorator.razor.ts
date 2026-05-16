import type { } from "@blazingstory/types/browser-dom";

export const dispatchComponentActionEvent = (name: string, argsJson: string) => {
    const event = new CustomEvent('componentaction', {
        cancelable: false,
        bubbles: true,
        detail: { name, argsJson }
    });
    const target = [...window.parent?.document.querySelectorAll('iframe')].find(f => f.contentWindow === window);
    target?.dispatchEvent(event);
}