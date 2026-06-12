using BlazingStory.Abstractions;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers.UserControllers;

/// <summary>
/// Context for passing to user controllers.
/// </summary>
public class UserControllerContext
{
    /// <summary>
    /// Gets or sets the unique key used to identify this controller instance.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the component parameter metadata this controller is bound to.
    /// </summary>
    public IComponentParameter Parameter { get; set; }

    /// <summary>
    /// Gets or sets the current value of the parameter.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the user changes the parameter value.
    /// </summary>
    public EventCallback<ParameterInputEventArgs> OnInput { get; set; }

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public UserControllerContext(string key, IComponentParameter param, object? value, EventCallback<ParameterInputEventArgs> oninput)
    {
        this.Key = key;
        this.Parameter = param;
        this.Value = value;
        this.OnInput = oninput;
    }
}
