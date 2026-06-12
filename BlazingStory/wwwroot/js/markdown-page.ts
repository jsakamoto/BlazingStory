// Resolved via phantom stub — see BlazingStory/BlazingStory.Toolkit/js/README.md
import { Prism } from "../../BlazingStory.ToolKit/js/prism.js";
import { injectCopyButton } from "./copybutton.js";

export const formatCodeBlock = (containerSelector: string): void => {
    const containers = document.querySelectorAll(containerSelector) as NodeListOf<HTMLElement>;
    containers.forEach((container) => {
        injectCopyButton(container);
        Prism.highlightElement(container.querySelector("code") as HTMLElement, false);
    });
}