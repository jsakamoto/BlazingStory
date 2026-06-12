type StyleDescriptor = {
    id?: string;
    enable?: boolean;
    href?: string;
};

const ensureStyleLink = (style: StyleDescriptor): void => {
    if (!style?.id) return;

    const id = `bs-style-${style.id}`;
    const existing = document.getElementById(id);

    if (!style.enable || !style.href) {
        existing?.remove();
        return;
    }

    if (existing instanceof HTMLLinkElement) {
        if (existing.href !== new URL(style.href, location.href).href) {
            existing.href = style.href;
        }
        return;
    }

    const link = document.createElement("link");
    link.id = id;
    link.rel = "stylesheet";
    link.href = style.href;
    document.head.appendChild(link);
};

export const ensurePreviewStyle = (
    background?: string,
    styleDescripters?: StyleDescriptor[],
    theme?: string,
    brand?: string,
): void => {
    if (background) {
        document.body.style.background = background;
    }

    if (theme) {
        document.body.dataset.bsTheme = theme;
    }

    if (brand) {
        document.body.dataset.bsBrand = brand;
    }

    for (const style of styleDescripters ?? []) {
        ensureStyleLink(style);
    }
};

export const dispatchComponentActionEvent = (
    name: string,
    argsJson: string,
): void => {
    const frameElement = window.frameElement as HTMLIFrameElement | null;
    window.parent.postMessage(
        {
            action: "component-action",
            frameId: frameElement?.id,
            name,
            argsJson,
        },
        location.origin,
    );
};
