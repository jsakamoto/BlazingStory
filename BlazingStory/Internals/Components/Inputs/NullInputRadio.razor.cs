using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.Inputs;

public partial class NullInputRadio : ComponentBase
{
    #region Public Properties

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public bool Checked { get; set; }

    [Parameter]
    public EventCallback OnChange { get; set; }

    #endregion Public Properties
}
