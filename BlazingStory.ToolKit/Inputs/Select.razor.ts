import type { IDisposable } from "@blazingstory/types/disposable";

/**
 * Prevent the default action of the keys used for the dropdown list navigation,
 * so that those keys do not move the caret inside the input element.
 */
export const preventDefaultOfNavigationKeys = (inputElement: HTMLInputElement): IDisposable => {

    const keydownListener = (ev: KeyboardEvent) => {
        if (ev.key === "ArrowDown" || ev.key === "ArrowUp" || ev.key === "F4") ev.preventDefault();
    }
    inputElement.addEventListener("keydown", keydownListener);
    return ({ dispose: () => inputElement.removeEventListener("keydown", keydownListener) });
}

export const scrollToHighlightedOption = (dropdownElement: HTMLElement | null): void => {
    dropdownElement?.querySelector(".highlighted")?.scrollIntoView({ block: "nearest" });
}
