namespace BlazingStory.Addons.BuiltIns.Toolbar.Grid;

/// <summary>
/// Registers the Grid toolbar content and its preview decorator with the addon builder.
/// </summary>
internal class GridAddon : IAddon
{
    /// <summary>
    /// Registers <see cref="GridToolbarContent"/> and <see cref="GridPreviewDecorator"/> with the provided builder.
    /// </summary>
    /// <param name="builder">The addon builder used to register toolbar content and decorators.</param>
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarContent<GridToolbarContent>(order: 100, match: viewMode => viewMode is ViewMode.Docs or ViewMode.Story);
        builder.AddPreviewDecorator<GridPreviewDecorator>();
    }
}
