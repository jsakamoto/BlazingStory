using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Models;

internal class ComponentParameter
{
    public readonly string Name;

    public readonly Type Type;

    public readonly string Summary;

    public readonly bool Required;

    internal ComponentParameter(PropertyInfo propertyInfo, string summary)
    {
        this.Name = propertyInfo.Name;
        this.Type = propertyInfo.PropertyType;
        this.Required = propertyInfo.GetCustomAttribute<EditorRequiredAttribute>() != null;
        this.Summary = summary;
    }
}
