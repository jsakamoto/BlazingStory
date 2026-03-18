namespace BlazingStory.Addons;

/// <summary>
/// Represents an addon that can register toolbar content, panels, and preview decorators.
/// </summary>
public interface IAddon
{
    /// <summary>
    /// Registers the addon's components with the provided <see cref="IAddonBuilder"/>.
    /// </summary>
    /// <param name="builder">The builder used to register addon components.</param>
    void Initialize(IAddonBuilder builder);
}
