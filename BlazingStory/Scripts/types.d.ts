export type CSSStyle = CSSStyleDeclaration & {
    zoom: string
}

export interface IDisposable {
    dispose(): void;
}

export interface DotNetObjectReference {
    invokeMethodAsync<T>(methodName: string, ...args: any[]): Promise<T>;
}

export type KeyEventArgument = {
    key: string,
    code: string,
    altKey: boolean,
    shiftKey: boolean,
    ctrlKey: boolean,
    metaKey: boolean,
};

export type MessageArgument =
    { action: "keydown", eventArgs: KeyEventArgument } |
    { action: "pointerdown" } |
    { action: "reload" };

declare global {

    interface IntersectionObserverInit {
        scrollMargin?: string;
    }

    interface Element {
        moveBefore?: (movedNode: Element, referenceNode: Element | null) => void;
    }
}
