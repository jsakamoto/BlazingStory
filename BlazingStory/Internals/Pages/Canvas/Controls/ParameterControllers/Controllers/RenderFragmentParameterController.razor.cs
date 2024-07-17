using System.Net;
using BlazingStory.Internals.Utils;

namespace BlazingStory.Internals.Pages.Canvas.Controls.ParameterControllers.Controllers;

public partial class RenderFragmentParameterController : ParameterControllerBase
{
    #region Private Fields

    private string? StringValue;

    #endregion Private Fields

    #region Protected Methods

    protected override void OnParametersSet()
    {
        var markupString = this.Value?.ToMarkupString();
        this.StringValue = WebUtility.HtmlDecode(markupString);
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task UpdateRenderFragmentAsync(string? internalValue)
    {
        this.StringValue = internalValue;

        var fragment = internalValue.ToRenderFragment();

        await this.OnInputAsync(fragment);
    }

    #endregion Private Methods
}
