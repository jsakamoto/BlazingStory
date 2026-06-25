const start = () => {
    const { promise: readyView, resolve: _setReadyView } = Promise.withResolvers();
    const { promise: readyAPI, resolve: _attachAPIRef } = Promise.withResolvers();
    window.BlazingStory = {
        _attachAPIRef,
        _setReadyView,
        readyView: () => readyView,
        getStoryIndex: async () => {
            const apiObject = await readyAPI;
            return await apiObject.invokeMethodAsync("GetStoryIndex");
        }
    };
};
export { start as beforeStart, start as beforeWebStart };
