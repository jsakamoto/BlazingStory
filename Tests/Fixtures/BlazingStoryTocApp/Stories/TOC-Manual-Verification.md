---
$attribute: CustomPage("Examples/TOC Manual Verification", TableOfContentsPlacement = global::BlazingStory.Types.TableOfContentsPlacement.Top)
---

# TOC Manual Verification

Use this page as a checklist when manually validating TOC behavior.

## Test Pages

- `Examples/TOC None`
- `Examples/TOC Duplicate Headings`
- `Examples/TOC Long Scroll`
- `Examples/TOC Manual Verification`

## Quick Routes

Use these direct URLs in the TOC fixture app (`https://localhost:5064`):

- None: `https://localhost:5064/?path=/custom/examples-toc-none`
- Top: `https://localhost:5064/?path=/custom/examples-toc-manual-verification`
- LeftSidebar: `https://localhost:5064/?path=/custom/examples-toc-duplicate-headings`
- RightSidebar: `https://localhost:5064/?path=/custom/examples-toc-long-scroll`

## Placement Override

TOC placement is configured per page via the `CustomPage` attribute.
The fixture app-level default is `TableOfContentsPlacement.None`.

Open each markdown file front matter and set `TableOfContentsPlacement` on the `$attribute: CustomPage(...)` line.

Available values:

- `TableOfContentsPlacement.None`
- `TableOfContentsPlacement.Top`
- `TableOfContentsPlacement.LeftSidebar`
- `TableOfContentsPlacement.RightSidebar`

## Verification Checklist

### Placement: None

- TOC is not rendered.
- Page content remains visible and scrollable.

### Placement: Top

- TOC is rendered at the top above markdown content.
- TOC links navigate to sections and update the URL hash.
- Scroll-spy is not active in top placement.

### Placement: LeftSidebar

- TOC appears in left sidebar on desktop.
- On narrow screens, sidebar TOC collapses to top block.
- Active section is highlighted while scrolling.

### Placement: RightSidebar

- TOC appears in right sidebar on desktop.
- On narrow screens, sidebar TOC collapses to top block.
- Active section is highlighted while scrolling.

## Duplicate Heading Checks

In `TOC Duplicate Headings`, verify that duplicate headings remain individually reachable via TOC links.

## Long Scroll Checks

In `TOC Long Scroll`, verify active-state progression while scrolling down and up.
