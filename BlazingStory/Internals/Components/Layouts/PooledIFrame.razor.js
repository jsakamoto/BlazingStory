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
const createIFrameViaFetch = async (src) => {
    const iframe = document.createElement('iframe');
    iframe.src = 'about:blank';
    // Try loading the iframe HTML via fetch to bypass broken response compression
    // (e.g. Visual Studio dev server's ERR_CONTENT_DECODING_FAILED issue).
    // We fetch index.html (which is served correctly) instead of iframe.html
    // (which may be corrupted by VS dev server compression).
    const baseUrl = new URL(src, document.baseURI);
    const candidates = [baseUrl.origin + baseUrl.pathname, baseUrl.origin + '/index.html'];
    for (const url of candidates) {
        try {
            const resp = await fetch(url);
            if (resp.ok) {
                let html = await resp.text();
                html = html.replace('<base href="/"', `<base href="${baseUrl.origin}/"`);
                iframe._fetchedHtml = html;
                iframe._targetSrc = src;
                break;
            }
        }
        catch { /* try next candidate */ }
    }
    if (!iframe._fetchedHtml) {
        iframe.src = src;
    }
    return iframe;
};
const writeHtmlToIFrame = (iframe) => {
    if (!iframe._fetchedHtml) return;
    const doc = iframe.contentDocument;
    doc.open();
    doc.write(iframe._fetchedHtml);
    doc.close();
    // Set the iframe location hash to carry the query parameters for Blazor routing.
    const targetUrl = new URL(iframe._targetSrc, document.baseURI);
    if (targetUrl.search) {
        iframe.contentWindow.history.replaceState(null, '', targetUrl.pathname + targetUrl.search);
    }
    delete iframe._fetchedHtml;
    delete iframe._targetSrc;
};
export const rent = async (containerElement, initialSrc, baseUri) => {
    const createIFrame = (src) => {
        const iframe = document.createElement('iframe');
        iframe.src = src;
        return iframe;
    };
    let iframe = iframePoolElement.querySelector('iframe') ?? await createIFrameViaFetch(initialSrc);
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
    writeHtmlToIFrame(iframe);
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
