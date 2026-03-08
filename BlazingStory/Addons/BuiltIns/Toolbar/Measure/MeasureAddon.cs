namespace BlazingStory.Addons.BuiltIns.Toolbar.Measure;

internal class MeasureAddon : IAddon
{
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarContent<MeasureToolbarContent>(order: 300, match: viewMode => viewMode is ViewMode.Story);
        builder.AddPreviewDecorator<MeasurePreviewDecorator>();
    }
}
