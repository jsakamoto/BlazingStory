namespace BlazingStory.Addons.BuiltIns.Panel.Actions;

internal class ActionsPanelAddon : IAddon
{
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddPanel<ActionsPanel>(order: 200, viewMode => viewMode == ViewMode.Story);
        builder.AddPreviewDecorator<ActionsPanelPreviewDecorator>();
    }
}
