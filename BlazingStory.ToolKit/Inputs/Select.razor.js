export const preventDefaultOfNavigationKeys = (inputElement) => {
    const keydownListener = (ev) => {
        if (ev.key === "ArrowDown" || ev.key === "ArrowUp" || ev.key === "F4")
            ev.preventDefault();
    };
    inputElement.addEventListener("keydown", keydownListener);
    return ({ dispose: () => inputElement.removeEventListener("keydown", keydownListener) });
};
export const scrollToHighlightedOption = (dropdownElement) => {
    dropdownElement?.querySelector(".highlighted")?.scrollIntoView({ block: "nearest" });
};
