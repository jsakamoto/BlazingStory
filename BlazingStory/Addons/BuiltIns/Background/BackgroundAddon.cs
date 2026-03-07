namespace BlazingStory.Addons.BuiltIns.Background;

internal class BackgroundAddon : IAddon
{
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarContent<BackgroundToolbarContent>(order: 200, match: viewMode => viewMode is ViewMode.Docs or ViewMode.Story);
        builder.AddPreviewDecorator<BackgroundPreviewDecorator>();
    }
}
