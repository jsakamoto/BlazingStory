declare global {
    interface IntersectionObserverInit {
        scrollMargin?: string;
    }

    interface Element {
        moveBefore?: (movedNode: Element, referenceNode: Element | null) => void;
    }

    interface CSSStyleDeclaration {
        zoom: string
    }
}
export { };