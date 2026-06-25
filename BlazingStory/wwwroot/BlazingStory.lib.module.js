import { BlazingStoryAPI } from "./js/BlazingStoryAPI.js";
const start = () => {
    window.BlazingStory = BlazingStoryAPI;
};
export { start as beforeStart, start as beforeWebStart };
