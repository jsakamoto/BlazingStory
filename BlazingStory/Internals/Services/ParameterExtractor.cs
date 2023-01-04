using System.Linq.Expressions;
using System.Reflection;
using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

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

    public static IEnumerable<ComponentParameter> GetParametersFromComponentType(Type componentType)
    {
        return componentType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.GetCustomAttribute<ParameterAttribute>() != null)
            .Select(prop => new ComponentParameter(prop, XmlDocComment.GetSummaryOfProperty(componentType, prop.Name)))
            .ToArray();
    }
}
