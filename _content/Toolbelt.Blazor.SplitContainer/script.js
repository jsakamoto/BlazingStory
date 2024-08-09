const pointerdown = "pointerdown";
const pointermove = "pointermove";
const pointerup = "pointerup";
const touchstart = "touchstart";
const NULL = null;
export const attach = (component, container) => {
    const state = {
        dir: 0,
        resizeTarget: -1,
        pivot: 0,
        initSize: 0,
        disposed: false,
        dispose: NULL
    };
    const spliter = container.querySelector(":scope > .spliter-bar");
    const panes = Array.from(container.querySelectorAll(":scope > .pane-of-split-container"));
    const round = Math.round;
    const getPos = (dir, ev) => round(dir === 0 ? ev.clientX : ev.clientY);
    const getSize = (dir, targetPaneIndex) => round(((rect) => dir === 0 ? rect.width : rect.height)(panes[targetPaneIndex].getBoundingClientRect()));
    const addEventListener = (element, event, callback, options) => {
        element.addEventListener(event, callback, options);
    };
    const removeEventListener = (element, event, callback) => {
        element.removeEventListener(event, callback);
    };
    const updateSize = (ev) => {
        const targetPaneIndex = state.resizeTarget;
        const resizeTarget = panes[targetPaneIndex] || NULL;
        if (resizeTarget === NULL)
            return [NULL, 0];
        const resizeTargetStyle = resizeTarget.style;
        const dir = state.dir;
        const currentPos = getPos(dir, ev);
        const delta = currentPos - state.pivot;
        const nextSize = (state.initSize + (targetPaneIndex == 0 ? +1 : -1) * delta) + "px";
        if (dir === 0)
            resizeTargetStyle.width = nextSize;
        else
            resizeTargetStyle.height = nextSize;
        return [resizeTarget, getSize(dir, targetPaneIndex)];
    };
    const onPointerMove = (ev) => { updateSize(ev); };
    const onPointerDown = (ev) => {
        if (!document.contains(spliter)) {
            state.dispose();
            return;
        }
        const targetPaneIndex = panes.findIndex(p => p.style.flex === "");
        if (targetPaneIndex === -1)
            return;
        const dir = container.classList.contains("splitter-orientation-vertical") ? 0 : 1;
        state.dir = dir;
        state.resizeTarget = targetPaneIndex;
        state.pivot = getPos(dir, ev);
        state.initSize = getSize(dir, targetPaneIndex);
        addEventListener(spliter, pointermove, onPointerMove);
        spliter.setPointerCapture(ev.pointerId);
    };
    const onPointerUp = (ev) => {
        spliter.releasePointerCapture(ev.pointerId);
        removeEventListener(spliter, pointermove, onPointerMove);
        const [resizeTarget, nextSize] = updateSize(ev);
        component.invokeMethodAsync("UpdateSize", resizeTarget === panes[0], nextSize);
    };
    const preventDefault = (ev) => ev.preventDefault();
    addEventListener(spliter, pointerdown, onPointerDown);
    addEventListener(spliter, pointerup, onPointerUp);
    addEventListener(spliter, touchstart, preventDefault, { passive: false });
    state.dispose = () => {
        if (state.disposed)
            return;
        state.disposed = true;
        removeEventListener(spliter, pointerdown, onPointerDown);
        removeEventListener(spliter, pointerup, onPointerUp);
        removeEventListener(spliter, touchstart, preventDefault);
    };
    return state;
};
