using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Pages.Canvas.Controls.ParameterControllers.Controllers;

public partial class EnumParameterController : ParameterControllerBase
{
    #region Private Properties

    private string ValueString => this.Value?.ToString() ?? "(null)";

    #endregion Private Properties

    #region Private Fields

    private string[] _EnumValues = Array.Empty<string>();

    #endregion Private Fields

    #region Protected Methods

    protected override void OnInitialized()
    {
        if (this.Parameter == null)
        {
            return;
        }

        var (isNullable, _, enumType, _) = this.Parameter.TypeStructure;
        var enumValues = Enum.GetValues(enumType).Cast<object>().Select(e => e.ToString() ?? "").ToArray();
        this._EnumValues = isNullable ? enumValues.Prepend("(null)").ToArray() : enumValues;
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task OnChange(ChangeEventArgs arg)
    {
        if (this.Parameter == null)
        {
            return;
        }

        var valueString = arg.Value?.ToString();

        if (string.IsNullOrEmpty(valueString) || valueString == "(null)")
        {
            if (this.Parameter.TypeStructure.IsNullable)
            {
                await this.OnInputAsync(null);
            }
        }
        else if (Enum.TryParse(this.Parameter.TypeStructure.PrimaryType, valueString, out var enumValue))
        {
            await this.OnInputAsync(enumValue);
        }
    }

    #endregion Private Methods
}
