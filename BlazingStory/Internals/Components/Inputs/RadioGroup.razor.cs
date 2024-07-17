using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.Inputs;

public partial class RadioGroup : ComponentBase
{
    #region Public Properties

    [Parameter, EditorRequired]
    public string? Name { get; set; }

    [Parameter, EditorRequired]
    public object? Value { get; set; }

    [Parameter, EditorRequired]
    public Array? Items { get; set; }

    [Parameter]
    public EventCallback<ChangeEventArgs> OnChange { get; set; }

    #endregion Public Properties
}
