export interface FrameHeightChangeEvent extends CustomEvent<{ height: number }> { }
export interface ComponentActionEvent extends CustomEvent<{ name: string, argsJson: string }> { }
export interface IntersectionChangeEvent extends CustomEvent<{ isIntersecting: boolean }> { }

export interface CustomEventMap {
    'intersectionchange': IntersectionChangeEvent;
    'frameheightchange': FrameHeightChangeEvent;
    'componentaction': ComponentActionEvent;
}

declare global {
    interface GlobalEventHandlersEventMap extends CustomEventMap { }
}