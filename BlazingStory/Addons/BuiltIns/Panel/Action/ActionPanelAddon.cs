namespace BlazingStory.Addons.BuiltIns.Panel.Action;

internal class ActionPanelAddon : IAddon
{
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddPanel<ActionPanel>(order: 200, viewMode => viewMode == ViewMode.Story);
        builder.AddPreviewDecorator<ActionPanelPreviewDecorator>();
    }
}
