using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Pages.Canvas.Controls.ParameterControllers.Controllers;

public class ParameterControllerBase : ComponentBase
{
    [Parameter, EditorRequired]
    public required string Key { get; set; }

    [Parameter, EditorRequired]
    public required ComponentParameter Parameter { get; set; }

    [Parameter, EditorRequired]
    public object? Value { get; set; }

    [Parameter]
    public EventCallback<ParameterInputEventArgs> OnInput { get; set; }

    protected async Task OnInputAsync(object? value)
    {
        if (this.Parameter == null) throw new NullReferenceException("Parameter is null.");
        await this.OnInput.InvokeAsync(new ParameterInputEventArgs(value, this.Parameter));
    }
}
