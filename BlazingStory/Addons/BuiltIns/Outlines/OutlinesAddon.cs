namespace BlazingStory.Addons.BuiltIns.Outlines;

internal class OutlinesAddon : IAddon
{
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarButton<OutlinesToolbarButton>(order: 400, viewMode => viewMode is ViewMode.Story or ViewMode.Docs);
        builder.AddPreviewDecorator<OutlinesPreviewDecorator>();
    }
}
