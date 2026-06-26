import type { BlazingStoryAPI } from "@blazingstory/types/blazing-story-api"
import type { CustomEventMap } from "./custom-events";

declare global {

    interface Window {
        /** The Blazor runtime object injected by the Blazor framework. */
        Blazor?: {
            /** Navigates the Blazor application to the specified URL. */
            navigateTo: (url: string) => void;
            /** Registers a custom DOM event type so Blazor can handle it as a component event. */
            registerCustomEventType: <TEvent extends Event = Event, TOut = void>(eventName: keyof CustomEventMap, args: { browserEventName?: string, createEventArgs?: (event: TEvent) => TOut }) => void;
        },

        /** The Blazing Story API object exposed on the browser window. */
        BlazingStory?: BlazingStoryAPI
    }

    /** The Blazor runtime object, accessible as a global variable. */
    const Blazor: Window['Blazor'];
    /** The Blazing Story API object, accessible as a global variable. */
    const BlazingStory: Window['BlazingStory'];
}

/** Represents a .NET object reference that can be used to invoke C# instance methods from JavaScript. */
export interface DotNetObjectReference {
    /** Invokes the named C# method asynchronously and returns the result. */
    invokeMethodAsync<T>(methodName: string, ...args: any[]): Promise<T>;
}
