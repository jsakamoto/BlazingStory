interface Window {
    Blazor?: {
        navigateTo: (url: string) => void;
        registerCustomEventType: <TEvent extends Event = Event, TOut>(eventName: string, args: { browserEventName?: string, createEventArgs?: (event: TEvent) => TOut }) => void;
    },
    BlazingStory?: {
        canvasFrameInitialized?: boolean;
    }
}
declare const Blazor: Window['Blazor'];