using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.XmlDocComment;
using Microsoft.AspNetCore.Components;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Services;

internal class ParameterExtractor
{
    public static string GetParameterName(Expression? expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (expression is not LambdaExpression lambda) throw new ArgumentException("expression is not a proprty expression.", nameof(expression));
        if (lambda.Body is not MemberExpression body) throw new ArgumentException("expression is not a proprty expression.", nameof(expression));
        return body.Member.Name;
    }

    public static IEnumerable<ComponentParameter> GetParametersFromComponentType([DynamicallyAccessedMembers(PublicProperties)] Type componentType, IXmlDocComment xmlDocComment)
    {
        return componentType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.GetCustomAttribute<ParameterAttribute>() != null)
            .Select(prop => new ComponentParameter(componentType, prop, xmlDocComment))
            .ToArray();
    }
}
