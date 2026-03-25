import { injectCopyButton } from "./copybutton.js";
import { Prism } from "./prism.js";

export const formatCodeBlock = (containerSelector: string): void => {
    const containers = document.querySelectorAll(containerSelector) as NodeListOf<HTMLElement>;
    containers.forEach((container) => {
        injectCopyButton(container);
        Prism.highlightElement(container.querySelector("code") as HTMLElement, false);
    });
}