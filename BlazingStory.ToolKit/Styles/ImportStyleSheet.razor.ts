export const importStyleSheet = (href: string, enable: boolean) => {
    const linkElement = document.createElement("link");
    linkElement.href = href;
    linkElement.rel = "stylesheet";
    document.head.appendChild(linkElement);
    return ({ dispose: () => linkElement.remove() })
}