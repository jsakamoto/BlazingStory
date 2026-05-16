export type KeyEventArgument = {
    key: string,
    code: string,
    altKey: boolean,
    shiftKey: boolean,
    ctrlKey: boolean,
    metaKey: boolean,
};

export type MessageArgument =
    { action: "keydown", eventArgs: KeyEventArgument } |
    { action: "pointerdown" } |
    { action: "reload" };
