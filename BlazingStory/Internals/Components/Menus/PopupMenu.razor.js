export const getPopupPos = (element) => {
    const rect = element.getBoundingClientRect();
    return ({ x: 0 ^ (rect.left + rect.width / 2), y: rect.bottom });
};

