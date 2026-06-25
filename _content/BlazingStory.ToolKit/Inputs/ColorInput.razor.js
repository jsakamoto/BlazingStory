export const getComputedColor = (colorText) => {
    const element = document.createElement("div");
    element.style.position = "fixed";
    element.style.width = "0";
    element.style.height = "0";
    element.style.color = colorText;
    document.body.appendChild(element);
    const computedColorText = getComputedStyle(element).color;
    element.remove();
    return computedColorText;
};
