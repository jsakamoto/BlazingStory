export const injectCopyButton = (containerSelectorOrElement) => {
    const containers = typeof containerSelectorOrElement === "string" ?
        document.querySelectorAll(containerSelectorOrElement) :
        [containerSelectorOrElement];
    containers.forEach((container) => {
        if (container.querySelector(".copy-button"))
            return;
        const copyButton = document.createElement("button");
        copyButton.className = "corner-button";
        copyButton.innerText = "Copy";
        const state = { timerId: -1 };
        copyButton.addEventListener("click", () => {
            const code = container.querySelector("code");
            const textToCopy = code.innerText;
            navigator.clipboard.writeText(textToCopy);
            copyButton.innerText = "Copied";
            if (state.timerId !== -1)
                clearTimeout(state.timerId);
            state.timerId = setTimeout(() => {
                copyButton.innerText = "Copy";
                state.timerId = -1;
            }, 1500);
        });
        container.appendChild(copyButton);
    });
};
