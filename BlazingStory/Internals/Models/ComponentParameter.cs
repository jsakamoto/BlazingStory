using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BlazingStory.Internals.Services.XmlDocComment;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Models;

internal class ComponentParameter
{
    private readonly Type _ComponentType;

    private readonly IXmlDocComment _XmlDocComment;

    private bool _Ready = false;

    internal readonly string Name;

    internal readonly Type Type;

    internal string Summary { get; private set; } = "";

    internal readonly bool Required;

    internal ControlType Control = ControlType.Default;

    internal object? DefaultValue = null;

    internal ComponentParameter(Type componentType, PropertyInfo propertyInfo, IXmlDocComment xmlDocComment)
    {
        this._ComponentType = componentType;
        this._XmlDocComment = xmlDocComment;
        this.Name = propertyInfo.Name;
        this.Type = propertyInfo.PropertyType;
        this.Required = propertyInfo.GetCustomAttribute<EditorRequiredAttribute>() != null;
    }

    internal async ValueTask EnsureReadyAsync()
    {
        Console.WriteLine("EnsureReadyAsync");
        if (this._Ready) return;
        this.Summary = await this._XmlDocComment.GetSummaryOfPropertyAsync(this._ComponentType, this.Name);
        this._Ready = true;
    }

    internal IEnumerable<string> GetParameterStrings()
    {
        if (this.Type.IsEnum)
        {
            yield return this.Type.Name;
            foreach (var enumValue in Enum.GetValues(this.Type))
            {
                yield return $"\"{enumValue}\"";
            }
        }
        else if (this.Type.IsGenericType)
        {
            yield return this.Type.UnderlyingSystemType.Name;
        }
        else
        {
            yield return this.Type.Name;
        }
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
