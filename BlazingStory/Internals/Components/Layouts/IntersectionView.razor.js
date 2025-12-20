const callback = (entries, observer) => {
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
export const observe = (element, dotNetObj) => {
    const handler = (event) => {
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
};
