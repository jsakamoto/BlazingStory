export const ensurePreviewStyle = (background, styleDescripters, theme) => {
    const doc = document;
    const head = doc.head;
    const bodyStyle = doc.body.style;
    if ('themableCanvasFrame' in window &&
        typeof window['themableCanvasFrame'] === 'function') {
        window['themableCanvasFrame'](theme);
    }
    else {
        console.log('themableCanvasFrame does not exist or is not a function on window');
    }
    bodyStyle.transition = 'background-color 0.3s';
    setTimeout(() => {
        bodyStyle.backgroundColor = background;
    }, 10);
    for (const descripter of styleDescripters) {
        const linkElement = head.querySelector(`link#${descripter.id}`);
        if (linkElement === null && descripter.enable) {
            const newLinkElement = doc.createElement('link');
            newLinkElement.id = descripter.id;
            newLinkElement.href = descripter.href;
            newLinkElement.rel = 'stylesheet';
            head.appendChild(newLinkElement);
        }
        else if (linkElement !== null && !descripter.enable) {
            linkElement.remove();
        }
    }
};
export const dispatchComponentActionEvent = (name, argsJson) => {
    const componentActionEventDetail = { name, argsJson };
    const event = new CustomEvent('componentActionEvent', {
        detail: componentActionEventDetail,
    });
    document.dispatchEvent(event);
};
