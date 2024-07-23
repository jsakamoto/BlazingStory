namespace BlazingStory.Internals.Pages.Canvas.Controls.ParameterControllers.Controllers;

public partial class BoolParameterController : ParameterControllerBase
{
    #region Private Fields

    private bool _BoolValue;

    #endregion Private Fields

    #region Protected Methods

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        this._BoolValue = bool.TryParse(this.Value?.ToString(), out var n) ? n : this._BoolValue;
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task OnChangeBoolValue()
    {
        this._BoolValue = !this._BoolValue;
        await this.OnInputAsync(this._BoolValue);
    }

    #endregion Private Methods
}
