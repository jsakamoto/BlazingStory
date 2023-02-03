import { DotNetObjectReference } from "../../../Scripts/types";

type SpacingSize = {
    top: number;
    left: number;
    bottom: number;
    right: number;
}

type Measurement = {
    boundary: DOMRect | null,
    padding?: SpacingSize,
    margin?: SpacingSize
}

const doc = document;
const targetEvents = [
    "pointerenter",
    "pointerover",
    "pointerleave",
    "scroll"
] as const;

let lastHoveredElement: HTMLElement | null = null;

let attachedOwner: DotNetObjectReference | null = null;

const pxToNumber = (px: string) => parseInt(px.replace("px", ""), 10);

const getSpacingSize = (style: CSSStyleDeclaration, prefix: "margin" | "padding"): SpacingSize => {
    const [top, left, bottom, right] = ["Top", "Left", "Bottom", "Right"].map(sufix => pxToNumber(style[prefix + sufix as any]));
    return { top, left, bottom, right };
}

const handler = (ev: MouseEvent | Event) => {
    if (attachedOwner === null) return;

    let hoveredElement = (ev instanceof MouseEvent) && document.elementFromPoint(ev.clientX, ev.clientY) as HTMLElement | null;

    let measurement: Measurement | null = null;
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
    else return;

    attachedOwner.invokeMethodAsync("TargetElementChanged", measurement);
}

export const attach = (owner: DotNetObjectReference) => {
    attachedOwner = owner;
    targetEvents.forEach(eventName => doc.addEventListener(eventName, handler));
}

export const detach = () => {
    attachedOwner = null;
    targetEvents.forEach(eventName => doc.removeEventListener(eventName, handler));
}