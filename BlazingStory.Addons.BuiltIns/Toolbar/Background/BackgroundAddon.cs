namespace BlazingStory.Addons.BuiltIns.Toolbar.Background;

/// <summary>
/// Registers the Background toolbar content and its preview decorator with the addon builder.
/// </summary>
internal class BackgroundAddon : IAddon
{
    /// <summary>
    /// Registers <see cref="BackgroundToolbarContent"/> and <see cref="BackgroundPreviewDecorator"/> with the provided builder.
    /// </summary>
    /// <param name="builder">The addon builder used to register toolbar content and decorators.</param>
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarContent<BackgroundToolbarContent>(order: 200, match: viewMode => viewMode is ViewMode.Docs or ViewMode.Story);
        builder.AddPreviewDecorator<BackgroundPreviewDecorator>();
    }
}
