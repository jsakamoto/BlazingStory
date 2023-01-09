using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Models;

internal class ComponentParameter
{
    internal readonly string Name;

    internal readonly Type Type;

    internal readonly string Summary;

    internal readonly bool Required;

    internal ControlType Control = ControlType.Default;

    internal object? DefaultValue = null;

    internal ComponentParameter(PropertyInfo propertyInfo, string summary)
    {
        this.Name = propertyInfo.Name;
        this.Type = propertyInfo.PropertyType;
        this.Required = propertyInfo.GetCustomAttribute<EditorRequiredAttribute>() != null;
        this.Summary = summary;
    }
}

internal static class ComponentParameterExtensoins
{
    public static bool TryGetByName(this IEnumerable<ComponentParameter> componentParameters, string name, [NotNullWhen(true)] out ComponentParameter? parameter)
    {
        parameter = componentParameters.FirstOrDefault(p => p.Name == name);
        return parameter != null;
    }
}
