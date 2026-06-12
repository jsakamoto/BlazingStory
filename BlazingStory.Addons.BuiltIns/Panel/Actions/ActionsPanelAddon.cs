namespace BlazingStory.Addons.BuiltIns.Panel.Actions;

/// <summary>
/// Registers the Actions panel and its preview decorator with the addon builder.
/// </summary>
internal class ActionsPanelAddon : IAddon
{
    /// <summary>
    /// Registers the <see cref="ActionsPanel"/> and <see cref="ActionsPanelPreviewDecorator"/> with the provided builder.
    /// </summary>
    /// <param name="builder">The addon builder used to register panels and decorators.</param>
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddPanel<ActionsPanel>(order: 200, viewMode => viewMode == ViewMode.Story);
        builder.AddPreviewDecorator<ActionsPanelPreviewDecorator>();
    }
}
