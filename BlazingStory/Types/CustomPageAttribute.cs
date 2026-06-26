namespace BlazingStory.Types;

/// <summary>
/// Marks a Razor component as a custom page and configures custom-page metadata.
/// </summary>
/// <param name="title">The slash-separated title used in the navigation tree.</param>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class CustomPageAttribute(string title) : Attribute
{
    /// <summary>
    /// Gets the slash-separated title for this custom page.
    /// </summary>
    public string? Title { get; init; } = title;

    /// <summary>
    /// Gets the per-page table of contents (TOC) placement override.
    /// When set to <see cref="Types.TableOfContentsPlacement.Inherit"/>, the global custom-page TOC placement setting is used.
    /// Any other value takes precedence over the global setting.
    /// </summary>
    public TableOfContentsPlacement TableOfContentsPlacement { get; init; } = Types.TableOfContentsPlacement.Inherit;

    /// <summary>
    /// Gets the per-page minimum heading level included in TOC.
    /// When <see langword="null"/>, the global default is used.
    /// </summary>
    public int? TableOfContentsMinHeadingLevel { get; init; }

    /// <summary>
    /// Gets the per-page maximum heading level included in TOC.
    /// When <see langword="null"/>, the global default is used.
    /// </summary>
    public int? TableOfContentsMaxHeadingLevel { get; init; }
}