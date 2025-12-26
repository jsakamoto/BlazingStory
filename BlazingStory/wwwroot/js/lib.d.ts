export interface FrameHeightChangeEvent extends CustomEvent<{ height: number }> { }
export interface ComponentActionEvent extends CustomEvent<{ name: string, argsJson: string }> { }
export interface IntersectionChangeEvent extends CustomEvent<{ isIntersecting: boolean }> { }

declare global {
    interface GlobalEventHandlersEventMap {
        'intersectionchange': IntersectionChangeEvent;
        'frameheightchange': FrameHeightChangeEvent;
        'componentActionEvent': ComponentActionEvent;
    }
}
