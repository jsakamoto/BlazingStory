using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Pages.Canvas.Controls.ParameterControllers.Controllers;

public class ParameterControllerBase : ComponentBase
{
    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    /// <value>
    /// The key.
    /// </value>
    [Parameter, EditorRequired]
    public string? Key { get; set; }

    /// <summary>
    /// Gets or sets the parameter.
    /// </summary>
    /// <value>
    /// The parameter.
    /// </value>
    [Parameter, EditorRequired]
    public ComponentParameter? Parameter { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    [Parameter, EditorRequired]
    public object? Value { get; set; }

    /// <summary>
    /// Gets or sets the options.
    /// </summary>
    /// <value>
    /// The options.
    /// </value>
    [Parameter]
    public string[]? Options { get; set; }

    /// <summary>
    /// Gets or sets the on input.
    /// </summary>
    /// <value>
    /// The on input.
    /// </value>
    [Parameter]
    public EventCallback<ParameterInputEventArgs> OnInput { get; set; }

    protected async Task OnInputAsync(object? value)
    {
        if (this.Parameter == null)
        {
            throw new NullReferenceException("Parameter is null.");
        }

        await this.OnInput.InvokeAsync(new ParameterInputEventArgs(value, this.Parameter));
    }
}
