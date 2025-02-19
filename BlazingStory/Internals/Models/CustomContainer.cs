using BlazingStory.Types;

namespace BlazingStory.Internals.Models;

/// <summary>
/// Represents a "custom component", container for custom pages.
/// </summary>
internal class CustomContainer
{
    internal readonly string Title;

    /// <summary>
    /// Gets a navigation path string for this custom page (component).<br/>
    /// (ex. "documentation-guides-setup")
    /// </summary>
    internal readonly string NavigationPath;

    public readonly CustomRazorDescriptor CustomRazorDescriptor;

    /// <summary>
    /// Initialize a new instance of <see cref="CustomContainer"/>.
    /// </summary>
    /// <param name="customRazorDescriptor">A descriptor of a type of Custom Razor component (..custom.razor) and its <see cref="CustomAttribute"/>.</param>
    public CustomContainer(CustomRazorDescriptor customRazorDescriptor)
    {
        this.CustomRazorDescriptor = customRazorDescriptor ?? throw new ArgumentNullException(nameof(customRazorDescriptor));
        this.Title = this.CustomRazorDescriptor.CustomAttribute.Title ?? throw new ArgumentNullException(nameof(customRazorDescriptor)); ;
        this.NavigationPath = Services.Navigation.NavigationPath.Create(this.Title);
    }

}