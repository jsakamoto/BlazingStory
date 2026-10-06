using BlazingStory.Addons;

namespace BlazingStory.Internals.Pages.Canvas;

/// <summary>
/// Registers the <see cref="CodePanel"/>, which shows the source code of the current story on the Canvas page, with the addon builder.
/// </summary>
internal class CodePanelAddon : IAddon
{
    /// <summary>
    /// Registers the <see cref="CodePanel"/> with the provided builder.
    /// </summary>
    /// <param name="builder">The addon builder used to register panels.</param>
    public void Initialize(IAddonBuilder builder)
    {
        builder.AddPanel<CodePanel>(order: 400, viewMode => viewMode is ViewMode.Story);
    }
}
