using BlazingStory.Abstractions;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers;

/// <summary>
/// Context for passing to parameter controllers.
/// </summary>
public class ParameterControllerContext
{
    /// <summary>
    /// Gets the unique key used to identify this controller instance.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the component parameter metadata this controller is bound to.
    /// </summary>
    public IComponentParameter Parameter { get; }

    /// <summary>
    /// Gets the current value of the parameter.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Gets the callback invoked when the user changes the parameter value.
    /// </summary>
    public EventCallback<ParameterInputEventArgs> OnInput { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterControllerContext"/> class.
    /// </summary>
    public ParameterControllerContext(string key, IComponentParameter param, object? value, EventCallback<ParameterInputEventArgs> oninput)
    {
        this.Key = key;
        this.Parameter = param;
        this.Value = value;
        this.OnInput = oninput;
    }
}
