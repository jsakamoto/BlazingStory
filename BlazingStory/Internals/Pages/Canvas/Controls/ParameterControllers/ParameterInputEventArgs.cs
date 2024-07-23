using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Pages.Canvas.Controls.ParameterControllers;

public struct ParameterInputEventArgs
{
    #region Internal Fields

    internal readonly object? Value;

    internal readonly ComponentParameter Parameter;

    #endregion Internal Fields

    #region Internal Constructors

    internal ParameterInputEventArgs(object? value, ComponentParameter parameter)
    {
        this.Value = value;
        this.Parameter = parameter;
    }

    #endregion Internal Constructors
}
