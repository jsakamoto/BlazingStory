using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BlazingStory.Abstractions;
using BlazingStory.Internals.Services.XmlDocComment;
using BlazingStory.ToolKit.Utils;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Models;

internal class ComponentParameter : IComponentParameter
{
    private readonly Type _ComponentType;

    private readonly PropertyInfo _PropertyInfo;

    private readonly IXmlDocComment _XmlDocComment;

    public string Name { get; }

    [DynamicallyAccessedMembers(PublicConstructors | PublicMethods | Interfaces | PublicProperties)]
    public Type Type { get; }

    public TypeStructure TypeStructure { get; }

    public MarkupString Summary { get; private set; } = default;

    public bool Required { get; }

    public ControlType Control { get; set; } = ControlType.Default;

    public object? DefaultValue { get; set; }

    internal ComponentParameter(Type componentType, PropertyInfo propertyInfo, IXmlDocComment xmlDocComment)
    {
        this._ComponentType = componentType;
        this._PropertyInfo = propertyInfo;
        this._XmlDocComment = xmlDocComment;
        this.Name = propertyInfo.Name;
#pragma warning disable IL2072, IL2074 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
        this.Type = propertyInfo.PropertyType;
        this.TypeStructure = TypeUtility.ExtractTypeStructure(propertyInfo.PropertyType);
#pragma warning restore IL2072, IL2074 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
        this.Required = propertyInfo.GetCustomAttribute<EditorRequiredAttribute>() != null;
    }

    /// <summary>
    /// Update summary property text of this parameter by reading a XML document comment file.
    /// </summary>
    public async ValueTask UpdateSummaryFromXmlDocCommentAsync()
    {
        this.Summary = await this._XmlDocComment.GetSummaryOfPropertyAsync(this._PropertyInfo.DeclaringType ?? this._ComponentType, this.Name);
    }

    public IEnumerable<string> GetParameterTypeStrings() => TypeUtility.GetTypeDisplayText(this.Type);
}

internal static class ComponentParameterExtensions
{
    public static bool TryGetByName(this IEnumerable<IComponentParameter> componentParameters, string name, [NotNullWhen(true)] out IComponentParameter? parameter)
    {
        parameter = componentParameters.FirstOrDefault(p => p.Name == name);
        return parameter != null;
    }
}
