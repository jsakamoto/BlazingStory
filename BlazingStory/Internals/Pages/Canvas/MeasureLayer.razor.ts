import type { DotNetObjectReference, IDisposable } from "../../../Scripts/types";

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

type Context = {
    owner: DotNetObjectReference,
    lastHoveredElement: HTMLElement | null
};

const pxToNumber = (px: string) => parseInt(px.replace("px", ""), 10);

const getSpacingSize = (style: CSSStyleDeclaration, prefix: "margin" | "padding"): SpacingSize => {
    const [top, left, bottom, right] = ["Top", "Left", "Bottom", "Right"].map(sufix => pxToNumber(style[prefix + sufix as any]));
    return { top, left, bottom, right };
}

const handler = (context: Context, ev: MouseEvent | Event) => {

    let hoveredElement = (ev instanceof MouseEvent) && document.elementFromPoint(ev.clientX, ev.clientY) as HTMLElement | null;

    let measurement: Measurement | null = null;
    if (context.lastHoveredElement !== null && hoveredElement === null) {
        context.lastHoveredElement = null;
    }
    else if (hoveredElement !== null && context.lastHoveredElement !== hoveredElement) {
        context.lastHoveredElement = hoveredElement ?? context.lastHoveredElement;
        if (context.lastHoveredElement !== null) {
            const computedStyle = window.getComputedStyle(context.lastHoveredElement);
            measurement = {
                boundary: context.lastHoveredElement.getBoundingClientRect(),
                padding: getSpacingSize(computedStyle, "padding"),
                margin: getSpacingSize(computedStyle, "margin"),
            };
        }
    }
    else return;

    context.owner.invokeMethodAsync("TargetElementChanged", measurement);
}

export const subscribeTargetElementChanged = (owner: DotNetObjectReference): IDisposable => {
    const context = { owner, lastHoveredElement: null as (HTMLElement | null) };
    const h = (ev: MouseEvent | Event) => handler(context, ev);
    targetEvents.forEach(eventName => doc.addEventListener(eventName, h));
    return ({ dispose: () => { targetEvents.forEach(eventName => doc.removeEventListener(eventName, h)); } });
}
