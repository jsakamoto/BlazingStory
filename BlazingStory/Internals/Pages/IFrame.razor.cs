using BlazingStory.Internals.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Pages;

public partial class IFrame : ComponentBase
{
    #region Private Properties

    [Inject] private IJSRuntime? JSRuntime { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender || this.JSRuntime is null)
        {
            return;
        }

        await using var module = await this.JSRuntime.ImportAsync("Internals/Pages/IFrame.razor.js");
        await module.InvokeVoidAsync("initializeCanvasFrame");
    }

    #endregion Protected Methods
}
