using BlazingStory.Abstractions;

namespace BlazingStory.Addons.BuiltIns.Panel.Controls.ParameterControllers;

public struct ParameterInputEventArgs
{
    internal readonly object? Value;

    internal readonly IComponentParameter Parameter;

    internal ParameterInputEventArgs(object? value, IComponentParameter parameter)
    {
        this.Value = value;
        this.Parameter = parameter;
    }
}
