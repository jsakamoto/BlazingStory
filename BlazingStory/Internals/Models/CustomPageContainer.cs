using BlazingStory.Types;
using BlazingStory.Internals.Pages.TableOfContents;

namespace BlazingStory.Internals.Models;

/// <summary>
/// Represents a "custom component", container for custom pages.
/// </summary>
internal class CustomPageContainer
{
    /// <summary>
    /// Gets the title of the custom page container.
    /// </summary>
    internal readonly string Title;

    /// <summary>
    /// Gets a navigation path string for this custom page (component).<br/>
    /// (ex. "documentation-guides-setup")
    /// </summary>
    internal readonly string NavigationPath;

    /// <summary>
    /// Gets the custom Razor descriptor and its metadata.
    /// </summary>
    public readonly CustomPageRazorDescriptor CustomPageRazorDescriptor;

    /// <summary>
    /// Initialize a new instance of <see cref="CustomPageContainer"/>.
    /// </summary>
    /// <param name="customPageRazorDescriptor">A descriptor of a type of Custom Razor component and its <see cref="CustomPageAttribute"/>.</param>
    public CustomPageContainer(CustomPageRazorDescriptor customPageRazorDescriptor)
    {
        this.CustomPageRazorDescriptor = customPageRazorDescriptor ?? throw new ArgumentNullException(nameof(customPageRazorDescriptor));
        this.Title = this.CustomPageRazorDescriptor.CustomPageAttribute.Title ?? throw new ArgumentNullException(nameof(customPageRazorDescriptor));
        this.NavigationPath = Services.Navigation.NavigationPath.Create(this.Title);
    }

    /// <summary>
    /// Resolves the effective TOC placement using page-level override and global default.
    /// </summary>
    /// <param name="globalDefaultPlacement">The application-level default TOC placement.</param>
    /// <returns>The effective TOC placement for this custom page.</returns>
    internal TableOfContentsPlacement ResolveTableOfContentsPlacement(TableOfContentsPlacement? globalDefaultPlacement)
    {
        var normalizedGlobalDefaultPlacement = NormalizeTableOfContentsPlacement(
            globalDefaultPlacement ?? TableOfContentsPlacement.None);
        var placement = this.CustomPageRazorDescriptor.CustomPageAttribute.TableOfContentsPlacement;
        var normalizedPlacement = NormalizeTableOfContentsPlacement(placement);
        return normalizedPlacement == TableOfContentsPlacement.Inherit ? normalizedGlobalDefaultPlacement : normalizedPlacement;
    }

    /// <summary>
    /// Resolves the effective minimum TOC heading level using page-level override and global default.
    /// </summary>
    /// <param name="globalDefaultMinHeadingLevel">The application-level default minimum heading level.</param>
    /// <returns>The effective minimum heading level for this custom page.</returns>
    internal int ResolveTableOfContentsMinHeadingLevel(int? globalDefaultMinHeadingLevel)
    {
        var value = this.CustomPageRazorDescriptor.CustomPageAttribute.TableOfContentsMinHeadingLevel
            ?? globalDefaultMinHeadingLevel
            ?? TableOfContentsDefaults.MinHeadingLevel;
        return TableOfContentsDefaults.NormalizeHeadingLevel(value);
    }

    /// <summary>
    /// Resolves the effective maximum TOC heading level using page-level override and global default.
    /// </summary>
    /// <param name="globalDefaultMaxHeadingLevel">The application-level default maximum heading level.</param>
    /// <returns>The effective maximum heading level for this custom page.</returns>
    internal int ResolveTableOfContentsMaxHeadingLevel(int? globalDefaultMaxHeadingLevel)
    {
        var value = this.CustomPageRazorDescriptor.CustomPageAttribute.TableOfContentsMaxHeadingLevel
            ?? globalDefaultMaxHeadingLevel
            ?? TableOfContentsDefaults.MaxHeadingLevel;
        return TableOfContentsDefaults.NormalizeHeadingLevel(value);
    }

    /// <summary>
    /// Normalizes a TOC placement value to a defined enum value.
    /// </summary>
    /// <param name="placement">The placement value to normalize.</param>
    /// <returns>A defined placement value.</returns>
    private static TableOfContentsPlacement NormalizeTableOfContentsPlacement(TableOfContentsPlacement placement)
    {
        return Enum.IsDefined(placement) ? placement : TableOfContentsPlacement.None;
    }

}