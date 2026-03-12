using BlazingStory.Internals.Models;

namespace BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers;

public struct ParameterInputEventArgs
{
    internal readonly object? Value;

    internal readonly ComponentParameter Parameter;

    internal ParameterInputEventArgs(object? value, ComponentParameter parameter)
    {
        this.Value = value;
        this.Parameter = parameter;
    }
}
