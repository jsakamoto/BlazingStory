import type { BlazingStoryAPI as BlazingStoryAPIType, StoryIndex } from "@blazingstory/types/blazing-story-api"
import type { DotNetObjectReference } from "../../../js/types/blazor";

const { promise: readyView, resolve: _setReadyView } = Promise.withResolvers<void>();
const { promise: readyAPI, resolve: _attachAPIRef } = Promise.withResolvers<DotNetObjectReference>();

export const BlazingStoryAPI: BlazingStoryAPIType = {
    _attachAPIRef,
    _setReadyView,
    readyView: () => readyView,
    getStoryIndex: async () => {
        const apiObject = await readyAPI;
        return await apiObject.invokeMethodAsync<StoryIndex>("GetStoryIndexAsync");
    }
}