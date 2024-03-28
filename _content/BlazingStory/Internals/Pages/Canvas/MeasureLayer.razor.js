const doc = document;
const targetEvents = [
    "pointerenter",
    "pointerover",
    "pointerleave",
    "scroll"
];
let lastHoveredElement = null;
let attachedOwner = null;
const pxToNumber = (px) => parseInt(px.replace("px", ""), 10);
const getSpacingSize = (style, prefix) => {
    const [top, left, bottom, right] = ["Top", "Left", "Bottom", "Right"].map(sufix => pxToNumber(style[prefix + sufix]));
    return { top, left, bottom, right };
};
const handler = (ev) => {
    if (attachedOwner === null)
        return;
    let hoveredElement = (ev instanceof MouseEvent) && document.elementFromPoint(ev.clientX, ev.clientY);
    let measurement = null;
    if (lastHoveredElement !== null && hoveredElement === null) {
        lastHoveredElement = null;
    }
    else if (hoveredElement !== null && lastHoveredElement !== hoveredElement) {
        lastHoveredElement = hoveredElement === false ? lastHoveredElement : hoveredElement;
        if (lastHoveredElement !== null) {
            const computedStyle = window.getComputedStyle(lastHoveredElement);
            measurement = {
                boundary: lastHoveredElement.getBoundingClientRect(),
                padding: getSpacingSize(computedStyle, "padding"),
                margin: getSpacingSize(computedStyle, "margin"),
            };
        }
    }
    else
        return;
    attachedOwner.invokeMethodAsync("TargetElementChanged", measurement);
};
export const attach = (owner) => {
    attachedOwner = owner;
    targetEvents.forEach(eventName => doc.addEventListener(eventName, handler));
};
export const detach = () => {
    attachedOwner = null;
    targetEvents.forEach(eventName => doc.removeEventListener(eventName, handler));
};
