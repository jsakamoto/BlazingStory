import type { DotNetObjectReference } from "@blazingstory/types/blazor";

/** Represents a single entry in the Storybook-compatible Story Index. */
export interface StoryIndexEntry {
    /** e.g."example-button--primary" */
    id: string,

    /** e.g."Example/Button" */
    title: string,

    /** e.g."Primary"*/
    name: string,

    /** The kind of entry: a rendered story or a docs page. */
    type: "docs" | "story"
}

/** Represents the Storybook-compatible Story Index returned by {@link BlazingStoryAPI.getStoryIndex}. */
export interface StoryIndex {
    /** Story Index format version. */
    v: number,
    /** All entries keyed by their ID. */
    entries: Record<string, StoryIndexEntry>
}

/** JavaScript-side interface for interacting with Blazing Story internals from the browser. */
export interface BlazingStoryAPI {
    /** Indicates whether the canvas frame has completed initialization. */
    canvasFrameInitialized?: boolean;
    /** Attaches the .NET object reference used to invoke C# methods from JavaScript. */
    _attachAPIRef: (x: DotNetObjectReference) => void,
    /** Signals that the ready view state has been reached. */
    _setReadyView: () => void,
    /** Resolves when Blazing Story has reached the ready view state. */
    readyView: () => Promise<void>,
    /** Returns the Storybook-compatible Story Index for all registered stories and pages. */
    getStoryIndex: () => Promise<StoryIndex>
}