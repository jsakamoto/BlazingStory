namespace BlazingStory.Addons.BuiltIns.Panel.Controls;

/// <summary>
/// Registers the Controls panel and its preview decorator with the addon builder.
/// </summary>
internal class ControlsPanelAddon : IAddon
{
    /// <summary>
    /// Registers the <see cref="ControlsPanel"/> and <see cref="ControlsPanelPreviewDecorator"/> with the provided builder.
    /// </summary>
    /// <param name="builder">The addon builder used to register panels and decorators.</param>
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddPanel<ControlsPanel>(order: 100, viewMode => viewMode is ViewMode.Story or ViewMode.Docs);
        builder.AddPreviewDecorator<ControlsPanelPreviewDecorator>();
    }
}
