using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.Buttons;

public partial class ToggleButton : ComponentBase
{
    #region Public Properties

    [Parameter]
    public bool Value { get; set; }

    [Parameter]
    public EventCallback<ChangeEventArgs> OnChange { get; set; }

    #endregion Public Properties
}
