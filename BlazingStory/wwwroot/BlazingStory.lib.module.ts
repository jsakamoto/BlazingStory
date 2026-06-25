import type { StoryIndex } from "@blazingstory/types/blazing-story-api"
import type { DotNetObjectReference } from "@blazingstory/types/blazor";

const start = () => {

    const { promise: readyView, resolve: _setReadyView } = Promise.withResolvers<void>();
    const { promise: readyAPI, resolve: _attachAPIRef } = Promise.withResolvers<DotNetObjectReference>();

    window.BlazingStory = {
        _attachAPIRef,
        _setReadyView,
        readyView: () => readyView,
        getStoryIndex: async () => {
            const apiObject = await readyAPI;
            return await apiObject.invokeMethodAsync<StoryIndex>("GetStoryIndex");
        }
    };
}

export {
    start as beforeStart,
    start as beforeWebStart
};