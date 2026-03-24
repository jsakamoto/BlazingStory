namespace BlazingStory.Addons.BuiltIns.Toolbar.Measure;

/// <summary>
/// Registers the Measure toolbar content and its preview decorator with the addon builder.
/// </summary>
internal class MeasureAddon : IAddon
{
    /// <summary>
    /// Registers <see cref="MeasureToolbarContent"/> and <see cref="MeasurePreviewDecorator"/> with the provided builder.
    /// </summary>
    /// <param name="builder">The addon builder used to register toolbar content and decorators.</param>
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarContent<MeasureToolbarContent>(order: 300, match: viewMode => viewMode is ViewMode.Story);
        builder.AddPreviewDecorator<MeasurePreviewDecorator>();
    }
}
