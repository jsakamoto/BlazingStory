namespace BlazingStory.Addons.BuiltIns.Toolbar.Outlines;

internal class OutlinesAddon : IAddon
{
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarContent<OutlinesToolbarContent>(order: 400, viewMode => viewMode is ViewMode.Story or ViewMode.Docs);
        builder.AddPreviewDecorator<OutlinesPreviewDecorator>();
    }
}
