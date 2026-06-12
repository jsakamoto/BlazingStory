using BlazingStory.Abstractions;

namespace BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers;

/// <summary>
/// Carries the new value and associated parameter metadata when a control input changes.
/// </summary>
public struct ParameterInputEventArgs
{
    /// <summary>
    /// The new value entered by the user.
    /// </summary>
    internal readonly object? Value;

    /// <summary>
    /// The component parameter whose control was changed.
    /// </summary>
    internal readonly IComponentParameter Parameter;

    /// <summary>
    /// Initializes a new instance of <see cref="ParameterInputEventArgs"/> with the supplied value and parameter.
    /// </summary>
    /// <param name="value">The new value provided by the user.</param>
    /// <param name="parameter">The component parameter associated with the input event.</param>
    internal ParameterInputEventArgs(object? value, IComponentParameter parameter)
    {
        this.Value = value;
        this.Parameter = parameter;
    }
}
