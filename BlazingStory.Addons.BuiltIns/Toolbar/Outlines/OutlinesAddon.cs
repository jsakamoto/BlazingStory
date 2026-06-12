namespace BlazingStory.Addons.BuiltIns.Toolbar.Outlines;

/// <summary>
/// Registers the Outlines toolbar content and its preview decorator with the addon builder.
/// </summary>
internal class OutlinesAddon : IAddon
{
    /// <summary>
    /// Registers <see cref="OutlinesToolbarContent"/> and <see cref="OutlinesPreviewDecorator"/> with the provided builder.
    /// </summary>
    /// <param name="builder">The addon builder used to register toolbar content and decorators.</param>
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarContent<OutlinesToolbarContent>(order: 400, viewMode => viewMode is ViewMode.Story or ViewMode.Docs);
        builder.AddPreviewDecorator<OutlinesPreviewDecorator>();
    }
}
