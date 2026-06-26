namespace BlazingStory.Addons.BuiltIns.Toolbar.Theme;

/// <summary>
/// Registers the theme toolbar content with the addon builder.
/// </summary>
internal class ThemeAddon : IAddon
{
    /// <summary>
    /// Registers <see cref="ThemeToolbarContent"/> with the provided builder.
    /// </summary>
    /// <param name="builder">The addon builder used to register toolbar content.</param>
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarContent<ThemeToolbarContent>(order: 50, viewMode => viewMode is ViewMode.Story or ViewMode.Docs or ViewMode.CustomPage);
    }
}
