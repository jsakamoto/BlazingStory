type StyleDescriptor = {
    id: string,
    href: string,
    enable: boolean
};

export const ensurePreviewStyle = (styleDescripters: StyleDescriptor[]) => {
    for (const descripter of styleDescripters) {
        const linkElement = document.head.querySelector(`link#${descripter.id}`);
        if (linkElement === null && descripter.enable) {
            const newLinkElement = document.createElement("link");
            newLinkElement.id = descripter.id;
            newLinkElement.href = descripter.href;
            newLinkElement.rel = "stylesheet";
            document.head.appendChild(newLinkElement);
        }
        else if (linkElement !== null && !descripter.enable) {
            linkElement.remove();
        }
    }
}
