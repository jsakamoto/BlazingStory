import type { IntersectionChangeEvent } from "../../../wwwroot/js/lib";

// Register custom event types
Blazor?.registerCustomEventType('intersectionchange', {
    createEventArgs: (e: IntersectionChangeEvent) => e.detail
});

export const observe = (element: HTMLElement): { dispose: () => void } => {

    const callback = (entries: IntersectionObserverEntry[]) => {
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
    observer.observe(element);
    return { dispose: () => observer.unobserve(element) };
}