import { injectCopyButton } from "./copybutton.js";
import { Prism } from "./prism.js";
export const formatCodeBlock = (containerSelector) => {
    const containers = document.querySelectorAll(containerSelector);
    containers.forEach((container) => {
        injectCopyButton(container);
        Prism.highlightElement(container.querySelector("code"), false);
    });
};
