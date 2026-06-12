import { Prism } from "../../BlazingStory.ToolKit/js/prism.js";
import { injectCopyButton } from "./copybutton.js";
export const formatCodeBlock = (containerSelector) => {
    const containers = document.querySelectorAll(containerSelector);
    containers.forEach((container) => {
        injectCopyButton(container);
        Prism.highlightElement(container.querySelector("code"), false);
    });
};
