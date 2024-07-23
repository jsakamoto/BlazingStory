using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Pages.Canvas.Controls.ParameterControllers;

public partial class ParameterController : ComponentBase
{
    #region Public Properties

    [Parameter, EditorRequired]
    public string? Key { get; set; }

    [Parameter, EditorRequired]
    public ComponentParameter? Parameter { get; set; }

    [Parameter, EditorRequired]
    public object? Value { get; set; }

    [Parameter]
    public EventCallback<ParameterInputEventArgs> OnInput { get; set; }

    #endregion Public Properties
}
