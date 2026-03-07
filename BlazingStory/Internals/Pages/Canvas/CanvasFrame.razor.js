export const ensurePreviewStyle = (styleDescripters) => {
    const doc = document;
    const head = doc.head;
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
};
export const dispatchComponentActionEvent = (name, argsJson) => {
    const event = new CustomEvent('componentaction', {
        cancelable: false,
        bubbles: true,
        detail: { name, argsJson }
    });
    const target = [...window.parent?.document.querySelectorAll('iframe')].find(f => f.contentWindow === window);
    target?.dispatchEvent(event);
};
