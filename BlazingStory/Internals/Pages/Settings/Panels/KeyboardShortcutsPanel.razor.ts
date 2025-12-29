export const init = (elementRef: HTMLElement) => {

    const handler = (e: KeyboardEvent) => {
        if (e.code !== "Tab" && e.code !== "Escape") return;
        e.stopPropagation();
    }

    elementRef.addEventListener("keydown", handler, true);

    return ({
        dispose: () => elementRef.removeEventListener("keydown", handler)
    });
}