export const importStyleSheet = (href, enable) => {
    const linkElement = document.createElement("link");
    linkElement.href = href;
    linkElement.rel = "stylesheet";
    document.head.appendChild(linkElement);
    return ({ dispose: () => linkElement.remove() });
};
