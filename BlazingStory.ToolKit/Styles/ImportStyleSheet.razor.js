export const importStyleSheet = (href) => {
    const linkElement = document.createElement("link");
    linkElement.href = href;
    linkElement.rel = "stylesheet";
    document.head.appendChild(linkElement);
    return ({ dispose: () => linkElement.remove() });
};
