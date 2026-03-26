const maxIframesInPool = 5;
const TTLForIFrameInPool = 1 * 60 * 1000;
const iframesInPool = Object.assign([], {
    remove: (iframe) => {
        const index = iframesInPool.findIndex(item => item.iframe === iframe);
        if (index >= 0)
            iframesInPool.splice(index, 1);
    }
});
const iframePoolElement = document.createElement('div');
Object.assign(iframePoolElement.style, {
    position: 'fixed',
    top: '0',
    left: '0',
    width: '0',
    height: '0',
    overflow: 'hidden'
});
document.body.appendChild(iframePoolElement);
class TimeoutError extends Error {
    constructor(message) { super(message); }
}
const waitFor = async (arg) => {
    let retryCount = 0;
    while (true) {
        const result = arg.predecate();
        if (result !== false)
            return result;
        if (retryCount >= (arg.maxRetryCount ?? 500))
            throw new TimeoutError("Timeout");
        retryCount++;
        await new Promise(resolve => setTimeout(resolve, arg.retryInterval ?? 10));
    }
};
const waitForIFrameReady = async (iframe) => {
    return await waitFor({
        predecate: () => {
            if (!iframe.contentWindow)
                return false;
            if (!iframe.contentWindow.Blazor)
                return false;
            return ({ contentWindow: iframe.contentWindow, blazor: iframe.contentWindow.Blazor });
        }
    });
};
export const rent = async (containerElement, initialSrc, baseUri) => {
    const createIFrame = (src) => {
        const iframe = document.createElement('iframe');
        iframe.src = src;
        return iframe;
    };
    const iframe = iframePoolElement.querySelector('iframe') ?? createIFrame(initialSrc);
    const iframeInPool = iframesInPool.find(item => item.iframe === iframe);
    if (iframeInPool) {
        clearTimeout(iframeInPool.TTLTimer);
        iframesInPool.remove(iframe);
    }
    if (!iframe.parentElement || !containerElement.moveBefore) {
        containerElement.appendChild(iframe);
    }
    else {
        containerElement.moveBefore(iframe, null);
    }
    const { contentWindow } = await waitForIFrameReady(iframe);
    if (contentWindow.location.href !== initialSrc) {
        await navigate(iframe, initialSrc, baseUri);
    }
};
export const release = (containerElement) => {
    const iframe = containerElement.querySelector('iframe');
    if (!iframe)
        return;
    const currentIFrameCount = iframePoolElement.querySelectorAll('iframe').length;
    if (currentIFrameCount >= maxIframesInPool || !iframePoolElement.moveBefore) {
        iframe.remove();
    }
    else {
        iframePoolElement.moveBefore(iframe, null);
        iframesInPool.push({
            iframe: iframe,
            TTLTimer: setTimeout(() => {
                iframe.remove();
                iframesInPool.remove(iframe);
            }, TTLForIFrameInPool)
        });
    }
};
export const navigate = async (containerOrIFrame, url, baseUri) => {
    const iframe = (containerOrIFrame.tagName !== "IFRAME" ? containerOrIFrame.querySelector("iframe") : containerOrIFrame);
    if (iframe === null)
        throw new Error("iframe not found");
    const { contentWindow, blazor } = await waitForIFrameReady(iframe);
    if (!contentWindow.location.href.startsWith(baseUri)) {
        contentWindow.location.href = url;
    }
    else {
        const navigateUrl = url.startsWith(baseUri) ? ("./" + url.substring(baseUri.length)) : url;
        blazor.navigateTo(navigateUrl);
    }
};
