export const dispatchComponentActionEvent = (
    name: string,
    argsJson: string,
) => {
    const event = new CustomEvent("componentaction", {
        cancelable: false,
        bubbles: true,
        detail: { name, argsJson },
    });
    const parentDocument = globalThis.parent?.document;
    const target = parentDocument
        ? [...parentDocument.querySelectorAll("iframe")].find(
              (f) => f.contentWindow === globalThis.window,
          )
        : null;
    if (target) {
        target.dispatchEvent(event);
        return;
    }

    globalThis.document.dispatchEvent(event);
};
