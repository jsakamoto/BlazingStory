const { promise: readyView, resolve: _setReadyView } = Promise.withResolvers();
const { promise: readyAPI, resolve: _attachAPIRef } = Promise.withResolvers();
export const BlazingStoryAPI = {
    _attachAPIRef,
    _setReadyView,
    readyView: () => readyView,
    getStoryIndex: async () => {
        const apiObject = await readyAPI;
        return await apiObject.invokeMethodAsync("GetStoryIndexAsync");
    }
};
