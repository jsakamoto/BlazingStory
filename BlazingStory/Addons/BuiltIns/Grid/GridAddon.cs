namespace BlazingStory.Addons.BuiltIns.Grid;

internal class GridAddon : IAddon
{
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarContent<GridToolbarContent>(order: 100, match: viewMode => viewMode is ViewMode.Docs or ViewMode.Story);
        builder.AddPreviewDecorator<GridPreviewDecorator>();
    }
}
