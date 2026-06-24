using BlazingStory.Types;

namespace BlazingStory.Internals.Pages.TableOfContents;

/// <summary>
/// Represents evaluated table-of-contents layout flags.
/// </summary>
/// <param name="RenderTop">Whether top TOC should render.</param>
/// <param name="RenderLeftSidebar">Whether left TOC sidebar should render.</param>
/// <param name="RenderRightSidebar">Whether right TOC sidebar should render.</param>
/// <param name="EnableScrollSpy">Whether scroll spy should be enabled.</param>
internal readonly record struct TableOfContentsLayoutState(
    bool RenderTop,
    bool RenderLeftSidebar,
    bool RenderRightSidebar,
    bool EnableScrollSpy);

/// <summary>
/// Evaluates table-of-contents layout state for custom pages.
/// </summary>
internal static class TableOfContentsLayoutEvaluator
{
    /// <summary>
    /// Evaluates the table-of-contents layout state.
    /// </summary>
    /// <param name="placement">The configured TOC placement.</param>
    /// <param name="isMarkdownCustomPage">Whether the page is markdown-based.</param>
    /// <param name="hasItems">Whether TOC items are available.</param>
    /// <returns>The evaluated layout state.</returns>
    internal static TableOfContentsLayoutState Evaluate(TableOfContentsPlacement placement, bool isMarkdownCustomPage, bool hasItems)
    {
        if (!isMarkdownCustomPage || !hasItems)
        {
            return new(
                RenderTop: false,
                RenderLeftSidebar: false,
                RenderRightSidebar: false,
                EnableScrollSpy: false);
        }

        return placement switch
        {
            TableOfContentsPlacement.Top => new(
                RenderTop: true,
                RenderLeftSidebar: false,
                RenderRightSidebar: false,
                EnableScrollSpy: false),

            TableOfContentsPlacement.LeftSidebar => new(
                RenderTop: false,
                RenderLeftSidebar: true,
                RenderRightSidebar: false,
                EnableScrollSpy: true),

            TableOfContentsPlacement.RightSidebar => new(
                RenderTop: false,
                RenderLeftSidebar: false,
                RenderRightSidebar: true,
                EnableScrollSpy: true),

            _ => new(
                RenderTop: false,
                RenderLeftSidebar: false,
                RenderRightSidebar: false,
                EnableScrollSpy: false),
        };
    }
}