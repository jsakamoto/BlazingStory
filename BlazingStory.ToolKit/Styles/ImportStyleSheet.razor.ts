export const importStyleSheet = (href: string) => {
    const linkElement = document.createElement("link");
    linkElement.href = href;
    linkElement.rel = "stylesheet";
    document.head.appendChild(linkElement);
    return ({ dispose: () => linkElement.remove() })
}