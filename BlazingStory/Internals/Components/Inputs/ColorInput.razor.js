const dig2hex = (dig) => ('0' + parseInt('' + dig).toString(16)).slice(-2);
const hex2num = (hex) => parseInt(hex, 16);

export const exchangeColor = (colorText) => {
    if (typeof (colorText) !== "string" || colorText === "") return colorText;

    const matchRgba = colorText.match(/^rgba?\((\d+)[ ,]+(\d+)[ ,]+(\d+)([ ,]+([\d\.%]+))?\)/i);
    const matchHex = colorText.match(/(^#([0-9a-f]{2})([0-9a-f]{2})([0-9a-f]{2})$|^#([0-9a-f])([0-9a-f])([0-9a-f])$)/i);
    const isNamed = matchRgba === null && matchHex === null;

    if (isNamed) {
        const element = document.createElement("div");
        element.style.position = "fixed";
        element.style.width = 0;
        element.style.height = 0;
        element.style.color = colorText;
        document.body.appendChild(element);
        const convertedColorText = getComputedStyle(element).color;
        element.remove();
        return convertedColorText;
    }

    if (matchRgba !== null) {
        const r = matchRgba[1];
        const g = matchRgba[2];
        const b = matchRgba[3];
        //const a = matchRgba[5];
        return `#${dig2hex(r)}${dig2hex(g)}${dig2hex(b)}`;
    }

    if (matchHex !== null) {
        const r = matchHex[2] || matchHex[5];
        const g = matchHex[3] || matchHex[6];
        const b = matchHex[4] || matchHex[7];
        return `rgba(${hex2num(r)}, ${hex2num(g)}, ${hex2num(b)}, 1.0)`;
    }

    return colorText;
}