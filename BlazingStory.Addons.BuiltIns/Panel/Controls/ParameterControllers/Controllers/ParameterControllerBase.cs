using BlazingStory.Abstractions;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers.Controllers;

/// <summary>
/// Base class for all parameter controller components, providing common parameters and an input notification helper.
/// </summary>
public class ParameterControllerBase : ComponentBase
{
    /// <summary>
    /// Gets or sets the unique key used to identify this controller instance.
    /// </summary>
    [Parameter, EditorRequired]
    public required string Key { get; set; }

    /// <summary>
    /// Gets or sets the component parameter metadata this controller is bound to.
    /// </summary>
    [Parameter, EditorRequired]
    public required IComponentParameter Parameter { get; set; }

    /// <summary>
    /// Gets or sets the current value of the parameter.
    /// </summary>
    [Parameter, EditorRequired]
    public object? Value { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the user changes the parameter value.
    /// </summary>
    [Parameter]
    public EventCallback<ParameterInputEventArgs> OnInput { get; set; }

    /// <summary>
    /// Invokes the <see cref="OnInput"/> callback with the supplied value wrapped in a <see cref="ParameterInputEventArgs"/>.
    /// </summary>
    /// <param name="value">The new value to report.</param>
    protected async Task OnInputAsync(object? value)
    {
        if (this.Parameter == null) throw new NullReferenceException("Parameter is null.");
        await this.OnInput.InvokeAsync(new ParameterInputEventArgs(value, this.Parameter));
    }
}
