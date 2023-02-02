const TargetElementChanged = "TargetElementChanged";
const doc = document;
let lastHoveredElement = null;
let attachedOwner = null;
const handler = (ev) => {
    if (attachedOwner === null)
        return;
    let hoveredElement = document.elementFromPoint(ev.clientX, ev.clientY);
    if (lastHoveredElement !== null && hoveredElement === null) {
        lastHoveredElement = null;
        attachedOwner.invokeMethodAsync(TargetElementChanged, null);
    }
    else if (hoveredElement !== null && lastHoveredElement !== hoveredElement) {
        lastHoveredElement = hoveredElement;
        const rect = hoveredElement.getBoundingClientRect();
        attachedOwner.invokeMethodAsync(TargetElementChanged, rect);
    }
};
export const attach = (owner) => {
    attachedOwner = owner;
    doc.addEventListener("mouseenter", handler);
    doc.addEventListener("mouseover", handler);
    doc.addEventListener("mouseleave", handler);
};
export const detach = () => {
    attachedOwner = null;
    doc.removeEventListener("mouseenter", handler);
    doc.removeEventListener("mouseover", handler);
    doc.removeEventListener("mouseleave", handler);
};
