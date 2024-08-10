using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BlazingStory.Internals.Services.XmlDocComment;
using BlazingStory.Internals.Utils;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Models;

public class ComponentParameter
{
    private readonly Type _ComponentType;

    private readonly PropertyInfo _PropertyInfo;

    private readonly IXmlDocComment _XmlDocComment;

    internal readonly string Name;

    [DynamicallyAccessedMembers(PublicConstructors | PublicMethods | Interfaces)]
    internal readonly Type Type;

    internal readonly TypeStructure TypeStructure;

    internal MarkupString Summary { get; private set; } = default;

    internal readonly bool Required;

    internal ControlType Control = ControlType.Default;

    internal object? DefaultValue = null;

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
    internal async ValueTask UpdateSummaryFromXmlDocCommentAsync()
    {
        this.Summary = await this._XmlDocComment.GetSummaryOfPropertyAsync(this._PropertyInfo.DeclaringType ?? this._ComponentType, this.Name);
    }

    internal IEnumerable<string> GetParameterTypeStrings() => TypeUtility.GetTypeDisplayText(this.Type);
}

internal static class ComponentParameterExtensions
{
    public static bool TryGetByName(this IEnumerable<ComponentParameter> componentParameters, string name, [NotNullWhen(true)] out ComponentParameter? parameter)
    {
        parameter = componentParameters.FirstOrDefault(p => p.Name == name);
        return parameter != null;
    }
}
