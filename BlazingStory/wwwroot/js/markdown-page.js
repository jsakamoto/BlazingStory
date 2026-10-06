import { Prism } from "../../BlazingStory.ToolKit/js/prism.js";
import { injectCopyButton } from "./copybutton.js";
let disposeTableOfContentsScrollSpy = null;
export const formatCodeBlock = (containerSelector) => {
    const containers = document.querySelectorAll(containerSelector);
    containers.forEach((container) => {
        injectCopyButton(container);
        Prism.highlightElement(container.querySelector("code"), false);
    });
};
export const collectHeadings = (containerSelector) => {
    const container = document.querySelector(containerSelector);
    if (container == null)
        return [];
    const headingElements = container.querySelectorAll("h1, h2, h3, h4, h5, h6");
    const idCounter = new Map();
    const headings = [];
    headingElements.forEach((heading) => {
        const level = Number.parseInt(heading.tagName.slice(1), 10);
        const text = (heading.textContent ?? "").trim();
        const candidate = heading.id.trim().length > 0 ? heading.id : text;
        const baseId = slugify(candidate);
        const id = ensureUniqueId(baseId, idCounter);
        heading.id = id;
        headings.push({ id, text, level });
    });
    return headings;
};
export const enableTableOfContentsScrollSpy = (containerSelector, tocSelector, headingSelector) => {
    disableTableOfContentsScrollSpy();
    const scrollContainer = document.querySelector(containerSelector);
    const toc = document.querySelector(tocSelector);
    if (scrollContainer == null || toc == null)
        return;
    const headingElements = Array.from(document.querySelectorAll(headingSelector)).filter((heading) => heading.id.trim().length > 0);
    if (headingElements.length === 0)
        return;
    const links = Array.from(toc.querySelectorAll(".toc-link[data-heading-id], .toc-item[data-heading-id]"));
    const setActive = (headingId) => {
        links.forEach((link) => {
            const isActive = headingId != null && link.dataset.headingId === headingId;
            link.classList.toggle("active", isActive);
            link.classList.toggle("toc-active", isActive);
            if (isActive)
                link.setAttribute("aria-current", "location");
            else
                link.removeAttribute("aria-current");
        });
    };
    const resolveActiveHeadingId = () => {
        const scrollContainerTop = scrollContainer.getBoundingClientRect().top;
        const activationOffset = 24;
        let lastPassedHeadingId = "";
        let nearestBelowHeadingId = headingElements[0]?.id ?? "";
        let nearestBelowDistance = Number.POSITIVE_INFINITY;
        headingElements.forEach((heading) => {
            const top = heading.getBoundingClientRect().top - scrollContainerTop;
            if (top <= activationOffset) {
                lastPassedHeadingId = heading.id;
                return;
            }
            const distance = top - activationOffset;
            if (distance < nearestBelowDistance) {
                nearestBelowDistance = distance;
                nearestBelowHeadingId = heading.id;
            }
        });
        return lastPassedHeadingId || nearestBelowHeadingId;
    };
    const updateActiveHeading = () => {
        const activeHeadingId = resolveActiveHeadingId();
        setActive(activeHeadingId);
    };
    const observer = new IntersectionObserver(() => updateActiveHeading(), {
        root: scrollContainer,
        threshold: [0, 1],
        rootMargin: "0px 0px -70% 0px",
    });
    headingElements.forEach((heading) => observer.observe(heading));
    scrollContainer.addEventListener("scroll", updateActiveHeading, {
        passive: true,
    });
    updateActiveHeading();
    disposeTableOfContentsScrollSpy = () => {
        observer.disconnect();
        scrollContainer.removeEventListener("scroll", updateActiveHeading);
        setActive(null);
    };
};
export const disableTableOfContentsScrollSpy = () => {
    if (disposeTableOfContentsScrollSpy == null)
        return;
    disposeTableOfContentsScrollSpy();
    disposeTableOfContentsScrollSpy = null;
};
const ensureUniqueId = (baseId, idCounter) => {
    const normalized = baseId.toLowerCase();
    const count = idCounter.get(normalized);
    if (count == null) {
        idCounter.set(normalized, 1);
        return baseId;
    }
    const next = count + 1;
    idCounter.set(normalized, next);
    return `${baseId}-${next}`;
};
const slugify = (source) => {
    const normalized = source
        .normalize("NFKD")
        .replace(/[\u0300-\u036f]/g, "")
        .toLowerCase();
    const slug = normalized.replace(/[^a-z0-9]+/g, "-").replace(/^-+|-+$/g, "");
    return slug.length > 0 ? slug : "section";
};
