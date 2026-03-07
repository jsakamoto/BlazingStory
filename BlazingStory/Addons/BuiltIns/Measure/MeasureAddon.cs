namespace BlazingStory.Addons.BuiltIns.Measure;

internal class MeasureAddon : IAddon
{
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddToolbarButton<MeasureToolbarButton>(order: 300, match: viewMode => viewMode is ViewMode.Story);
        builder.AddPreviewDecorator<MeasurePreviewDecorator>();
    }
}
