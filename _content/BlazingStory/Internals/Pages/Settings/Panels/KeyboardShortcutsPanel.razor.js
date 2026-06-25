export const init = (elementRef) => {
    const handler = (e) => {
        if (e.code !== "Tab" && e.code !== "Escape")
            return;
        e.stopPropagation();
    };
    elementRef.addEventListener("keydown", handler, true);
    return ({
        dispose: () => elementRef.removeEventListener("keydown", handler)
    });
};
