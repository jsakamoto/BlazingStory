export const subscribeComponentAction = async (owner, callbackMethodName) => {
    const handler = async (e) => await owner.invokeMethodAsync(callbackMethodName, e.detail);
    document.addEventListener('componentaction', handler);
    return ({ dispose: () => { document.removeEventListener('componentaction', handler); } });
};
