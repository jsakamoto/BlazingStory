export type HighlightCallback = (element: HTMLElement) => void;

export const Prism: {
    highlightAll: () => void;
    highlightElement: (element: HTMLElement, async?: boolean, callback?: HighlightCallback) => void;
};
