import type { DotNetObjectReference } from "../../../Scripts/types";

declare global {
    interface HTMLElementEventMap {
        'intersectionchange': CustomEvent<{ isIntersecting: boolean }>;
    }
}

const callback = (entries: IntersectionObserverEntry[], observer: IntersectionObserver) => {
    entries.forEach(entry => {
        const customEvent = new CustomEvent('intersectionchange', {
            cancelable: false,
            bubbles: true,
            detail: { isIntersecting: entry.isIntersecting }
        });
        entry.target.dispatchEvent(customEvent);
    });
};

const observer = new IntersectionObserver(callback, {
    root: null,
    scrollMargin: '50px',
    threshold: 0
});

export const observe = (element: HTMLElement, dotNetObj: DotNetObjectReference): { dispose: () => void } => {
    const handler = (event: CustomEvent) => {
        dotNetObj.invokeMethodAsync('OnIntersectionChange', event.detail.isIntersecting);
    };
    element.addEventListener('intersectionchange', handler);
    observer.observe(element);
    return {
        dispose: async () => {
            element.removeEventListener('intersectionchange', handler);
            observer.unobserve(element);
        }
    };
}