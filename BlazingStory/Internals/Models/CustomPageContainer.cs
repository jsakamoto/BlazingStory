using BlazingStory.Types;

namespace BlazingStory.Internals.Models;

/// <summary>
/// Represents a "custom component", container for custom pages.
/// </summary>
internal class CustomPageContainer
{
    internal readonly string Title;

    /// <summary>
    /// Gets a navigation path string for this custom page (component).<br/>
    /// (ex. "documentation-guides-setup")
    /// </summary>
    internal readonly string NavigationPath;

    public readonly CustomPageRazorDescriptor CustomPageRazorDescriptor;

    /// <summary>
    /// Initialize a new instance of <see cref="CustomPageContainer"/>.
    /// </summary>
    /// <param name="customPageRazorDescriptor">A descriptor of a type of Custom Razor component and its <see cref="CustomPageAttribute"/>.</param>
    public CustomPageContainer(CustomPageRazorDescriptor customPageRazorDescriptor)
    {
        this.CustomPageRazorDescriptor = customPageRazorDescriptor ?? throw new ArgumentNullException(nameof(customPageRazorDescriptor));
        this.Title = this.CustomPageRazorDescriptor.CustomPageAttribute.Title ?? throw new ArgumentNullException(nameof(customPageRazorDescriptor)); ;
        this.NavigationPath = Services.Navigation.NavigationPath.Create(this.Title);
    }

}