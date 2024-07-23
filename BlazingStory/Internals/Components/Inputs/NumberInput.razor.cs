using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazingStory.Internals.Components.Inputs;

public partial class NumberInput : ComponentBase
{
    #region Public Properties

    [Parameter]
    public int Value { get; set; }

    [Parameter]
    public string? PlaceHolder { get; set; }

    [Parameter]
    public EventCallback<ChangeEventArgs> OnInput { get; set; }

    [Parameter]
    public EventCallback<FocusEventArgs> OnFocus { get; set; }

    #endregion Public Properties
}
