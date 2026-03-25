export const setBackgroundColor = (color) => {
    const bodyStyle = document.body.style;
    bodyStyle.transition = "background-color 0.3s";
    setTimeout(() => { bodyStyle.backgroundColor = color; }, 10);
};
