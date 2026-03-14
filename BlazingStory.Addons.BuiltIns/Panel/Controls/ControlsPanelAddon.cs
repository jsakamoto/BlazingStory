namespace BlazingStory.Addons.BuiltIns.Panel.Controls;

internal class ControlsPanelAddon : IAddon
{
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddPanel<ControlsPanel>(order: 100, viewMode => viewMode is ViewMode.Story or ViewMode.Docs);
        builder.AddPreviewDecorator<ControlsPanelPreviewDecorator>();
    }
}
