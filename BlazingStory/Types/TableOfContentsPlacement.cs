namespace BlazingStory.Types;

/// <summary>
/// Specifies where the table of contents (TOC) is rendered on markdown-based custom pages.
/// </summary>
public enum TableOfContentsPlacement
{
    /// <summary>
    /// Inherit the global custom-page TOC placement setting.
    /// </summary>
    Inherit = -1,

    /// <summary>
    /// The TOC is not rendered.
    /// </summary>
    None,

    /// <summary>
    /// The TOC is rendered above the page content.
    /// </summary>
    Top,

    /// <summary>
    /// The TOC is rendered in a left sidebar.
    /// </summary>
    LeftSidebar,

    /// <summary>
    /// The TOC is rendered in a right sidebar.
    /// </summary>
    RightSidebar,
}