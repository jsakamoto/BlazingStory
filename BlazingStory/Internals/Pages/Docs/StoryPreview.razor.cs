using BlazingStory.Internals.Components.Preview;
using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Pages.Docs;

public partial class StoryPreview : ComponentBase
{
    #region Public Properties

    [Parameter, EditorRequired]
    public Story? Story { get; set; }

    [Parameter]
    public bool EnableZoom { get; set; }

    [Parameter]
    public IReadOnlyDictionary<string, object?>? Globals { get; set; }

    #endregion Public Properties

    #region Internal Properties

    [CascadingParameter]
    internal IServiceProvider Services { get; init; } = default!;

    #endregion Internal Properties

    #region Private Fields

    private PreviewFrame? _PreviewFrame;

    private bool _ShowCode = false;

    #endregion Private Fields

    #region Private Methods

    private async void OnClickZoomIn()
    {
        if (this._PreviewFrame != null)
        {
            await this._PreviewFrame.ZoomInAsync();
        }
    }

    private async void OnClickZoomOut()
    {
        if (this._PreviewFrame != null)
        {
            await this._PreviewFrame.ZoomOutAsync();
        }
    }

    private async void OnClickResetZoom()
    {
        if (this._PreviewFrame != null)
        {
            await this._PreviewFrame.ResetZoomAsync();
        }
    }

    #endregion Private Methods
}
