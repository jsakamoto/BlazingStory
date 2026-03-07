namespace BlazingStory.Addons.BuiltIns.Grid;

internal class GridAddon : IAddon
{
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarButton<GridToolbarButton>(order: 100, match: viewMode => viewMode is ViewMode.Docs or ViewMode.Story);
        //builder.AddPreviewExtension<GridPreviewExtension>();
    }
}
