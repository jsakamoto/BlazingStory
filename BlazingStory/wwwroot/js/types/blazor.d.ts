import type { CustomEventMap } from "./custom-events";

declare global {

    interface Window {
        Blazor?: {
            navigateTo: (url: string) => void;
            registerCustomEventType: <TEvent extends Event = Event, TOut>(eventName: keyof CustomEventMap, args: { browserEventName?: string, createEventArgs?: (event: TEvent) => TOut }) => void;
        },
        BlazingStory?: {
            canvasFrameInitialized?: boolean;
        }
    }

    declare const Blazor: Window['Blazor'];
}

export interface DotNetObjectReference {
    invokeMethodAsync<T>(methodName: string, ...args: any[]): Promise<T>;
}
