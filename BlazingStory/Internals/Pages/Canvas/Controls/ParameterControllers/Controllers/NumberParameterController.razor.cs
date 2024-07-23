using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Pages.Canvas.Controls.ParameterControllers.Controllers;

public partial class NumberParameterController : ParameterControllerBase
{
    #region Private Fields

    private int _NumValue;

    #endregion Private Fields

    #region Protected Methods

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this._NumValue = int.TryParse(this.Value?.ToString(), out var n) ? n : this._NumValue;
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task OnInputNumValue(ChangeEventArgs arg)
    {
        if (int.TryParse(arg.Value?.ToString(), out var n))
        {
            this._NumValue = n;
            await this.OnInputAsync(n);
        }
    }

    #endregion Private Methods
}
