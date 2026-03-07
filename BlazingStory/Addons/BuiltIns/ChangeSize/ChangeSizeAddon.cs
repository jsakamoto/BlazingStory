namespace BlazingStory.Addons.BuiltIns.ChangeSize;

internal class ChangeSizeAddon : IAddon
{
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarContent<ChangeSizeToolbarContent>(order: 500, viewMode => viewMode is ViewMode.Story);
    }
}
