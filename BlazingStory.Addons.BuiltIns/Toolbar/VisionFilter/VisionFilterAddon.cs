namespace BlazingStory.Addons.BuiltIns.Toolbar.VisionFilter;

internal class VisionFilterAddon : IAddon
{
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarContent<VisionFilterToolbarContent>(order: 600, viewMode => viewMode is ViewMode.Story);
        builder.AddPreviewDecorator<VisionFilterPreviewDecorator>();
    }
}
