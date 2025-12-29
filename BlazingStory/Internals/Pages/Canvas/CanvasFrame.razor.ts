import type { } from "../../../wwwroot/js/types/browser-dom";

type StyleDescriptor = {
    id: string,
    href: string,
    enable: boolean
};

export const ensurePreviewStyle = (background: string, styleDescripters: StyleDescriptor[]) => {
    const doc = document;
    const head = doc.head;
    const bodyStyle = doc.body.style;

    bodyStyle.transition = "background-color 0.3s";
    setTimeout(() => { bodyStyle.backgroundColor = background; }, 10);


    for (const descripter of styleDescripters) {
        const linkElement = head.querySelector(`link#${descripter.id}`);
        if (linkElement === null && descripter.enable) {
            const newLinkElement = doc.createElement("link");
            newLinkElement.id = descripter.id;
            newLinkElement.href = descripter.href;
            newLinkElement.rel = "stylesheet";
            head.appendChild(newLinkElement);
        }
        else if (linkElement !== null && !descripter.enable) {
            linkElement.remove();
        }
    }
}

export const dispatchComponentActionEvent = (name: string, argsJson: string) => {
    const event = new CustomEvent('componentaction', {
        cancelable: false,
        bubbles: true,
        detail: { name, argsJson }
    });
    const target = [...window.parent?.document.querySelectorAll('iframe')].find(f => f.contentWindow === window);
    target?.dispatchEvent(event);
}