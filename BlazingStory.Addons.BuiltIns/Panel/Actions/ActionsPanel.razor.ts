import type { DotNetObjectReference } from "@blazingstory/types/blazor";
import type { IDisposable } from "@blazingstory/types/disposable";
import type { ComponentActionEvent } from "@blazingstory/types/custom-events";

export const subscribeComponentAction = async (owner: DotNetObjectReference, callbackMethodName: string): Promise<IDisposable> => {
    const handler = async (e: ComponentActionEvent) => await owner.invokeMethodAsync(callbackMethodName, e.detail);
    document.addEventListener('componentaction', handler);
    return ({ dispose: () => { document.removeEventListener('componentaction', handler); } });
}