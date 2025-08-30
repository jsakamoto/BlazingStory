const doc = document;
const targetEvents = [
    "pointerenter",
    "pointerover",
    "pointerleave",
    "scroll"
];
const pxToNumber = (px) => parseInt(px.replace("px", ""), 10);
const getSpacingSize = (style, prefix) => {
    const [top, left, bottom, right] = ["Top", "Left", "Bottom", "Right"].map(sufix => pxToNumber(style[prefix + sufix]));
    return { top, left, bottom, right };
};
const handler = (context, ev) => {
    let hoveredElement = (ev instanceof MouseEvent) && document.elementFromPoint(ev.clientX, ev.clientY);
    let measurement = null;
    if (context.lastHoveredElement !== null && hoveredElement === null) {
        context.lastHoveredElement = null;
    }
    else if (hoveredElement !== null && context.lastHoveredElement !== hoveredElement) {
        context.lastHoveredElement = hoveredElement === false ? context.lastHoveredElement : hoveredElement;
        if (context.lastHoveredElement !== null) {
            const computedStyle = window.getComputedStyle(context.lastHoveredElement);
            measurement = {
                boundary: context.lastHoveredElement.getBoundingClientRect(),
                padding: getSpacingSize(computedStyle, "padding"),
                margin: getSpacingSize(computedStyle, "margin"),
            };
        }
    }
    else
        return;
    context.owner.invokeMethodAsync("TargetElementChanged", measurement);
};
export const subscribeTargetElementChanged = (owner) => {
    const context = { owner, lastHoveredElement: null };
    const h = (ev) => handler(context, ev);
    targetEvents.forEach(eventName => doc.addEventListener(eventName, h));
    return ({ dispose: () => { targetEvents.forEach(eventName => doc.removeEventListener(eventName, h)); } });
};
