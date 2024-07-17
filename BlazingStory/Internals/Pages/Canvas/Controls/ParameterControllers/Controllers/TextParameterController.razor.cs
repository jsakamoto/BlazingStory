using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Pages.Canvas.Controls.ParameterControllers.Controllers;

public partial class TextParameterController : ParameterControllerBase
{
    #region Private Fields

    private string? _TextValue;

    #endregion Private Fields

    #region Protected Methods

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this._TextValue = this.Value?.ToString();
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task OnInputTextValue(ChangeEventArgs arg)
    {
        this._TextValue = arg.Value?.ToString();
        await this.OnInputAsync(this._TextValue);
    }

    #endregion Private Methods
}
