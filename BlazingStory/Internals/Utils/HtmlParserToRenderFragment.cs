using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazingStory.Internals.Utils;

internal static class HtmlParserToRenderFragment
{
    private static readonly Lazy<MethodInfo> AddContentMethod = new(() => typeof(RenderTreeBuilder).GetMethod(nameof(RenderTreeBuilder.AddContent), [typeof(int), typeof(string)]) ?? throw new InvalidOperationException());

    private static readonly Lazy<ParameterExpression> BuilderParam = new(() => Expression.Parameter(typeof(RenderTreeBuilder), "builder"));

    /// <summary>
    /// Convert the given string to <see cref="RenderFragment" />.
    /// </summary>
    /// <param name="internalValue">
    /// The string content what is the <see cref="RenderFragment" /> will render.
    /// </param>
    /// <returns>
    /// The <see cref="RenderFragment" /> that will render the given string content.
    /// </returns>
    internal static RenderFragment ToRenderFragment(this string? internalValue)
    {
        RenderFragment fragment;

        var tags = internalValue?.ParseMarkupString();

        if (tags is not null && tags.Any())
        {
            fragment = builder =>
            {
                var sequence = 0;
                foreach (var tag in tags)
                {
                    RenderElement(builder, tag, ref sequence);
                }
            };
        }
        else
        {
            fragment = builder => { builder.AddMarkupContent(0, internalValue); };
        }

        return fragment;
    }

    /// <summary>
    /// Convert the given string to <see cref="RenderFragment&lt;TValue&gt;" />.
    /// </summary>
    /// <param name="argumentType">
    /// The type argument of <see cref="RenderFragment&lt;TValue&gt;" />.
    /// </param>
    /// <param name="text">
    /// The string content what is the <see cref="RenderFragment&lt;TValue&gt;" /> will render.
    /// </param>
    /// <returns>
    /// The <see cref="RenderFragment&lt;TValue&gt;" /> that will render the given string content.
    /// </returns>
    internal static object ToRenderFragment(this string text, Type argumentType)
    {
        var addContentCall = Expression.Call(BuilderParam.Value, AddContentMethod.Value, Expression.Constant(0), Expression.Constant(text));
        var renderFragment = Expression.Lambda(typeof(RenderFragment), addContentCall, BuilderParam.Value);

        var argParam = Expression.Parameter(argumentType, "arg");
        var renderFragmentTDelegateType = typeof(RenderFragment<>).MakeGenericType(argumentType);
        var renderFragmentT = Expression.Lambda(renderFragmentTDelegateType, renderFragment, argParam);

        var result = renderFragmentT.Compile();

        return result;
    }

    [SuppressMessage("Trimming", "IL2072:Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.", Justification = "<Pending>")]
    private static void RenderElement(RenderTreeBuilder builder, HtmlElement element, ref int sequence)
    {
        // If there's no tag name, just render the inner HTML or children
        if (string.IsNullOrWhiteSpace(element.TagName))
        {
            if (!string.IsNullOrWhiteSpace(element.Content) && element.Children is not null && element.Children.Any())
            {
                foreach (var child in element.Children)
                {
                    RenderElement(builder, child, ref sequence);
                }
            }

            return;
        }

        var isHtmlTag = element.TagName.IsHtmlTag();
        var componentType = !isHtmlTag ? element.TagName.FindComponentType() : null;

        if (isHtmlTag)
        {
            RenderHtmlElement(builder, element, ref sequence);
        }
        else if (componentType != null || element.TagName.Equals(nameof(EditForm)))
        {
            if (componentType != null)
            {
                RenderBlazorComponent(builder, element, componentType, ref sequence);
            }
            else
            {
                RenderBlazorComponent(builder, element, typeof(EditForm), ref sequence);
            }
        }
        else
        {
            RenderHtmlElement(builder, element, ref sequence);
        }
    }

    private static void RenderHtmlElement(RenderTreeBuilder builder, HtmlElement element, ref int sequence)
    {
        // If there's no tag name, just render the inner HTML or children
        if (string.IsNullOrWhiteSpace(element.TagName))
        {
            if (!string.IsNullOrWhiteSpace(element.Content) && element.Children is not null && element.Children.Any())
            {
                foreach (var child in element.Children)
                {
                    RenderElement(builder, child, ref sequence);
                }
            }

            return;
        }

        builder.OpenElement(sequence++, element.TagName);

        if (element.Attributes != null)
        {
            foreach (var attribute in element.Attributes)
            {
                AddAttribute(builder, ref sequence, attribute.Key, attribute.Value);
            }
        }

        if (element.Children is not null && element.Children.Any())
        {
            foreach (var child in element.Children)
            {
                RenderElement(builder, child, ref sequence);
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(element.Content))
            {
                builder.AddContent(sequence++, element.Content);
            }
        }

        builder.CloseElement();
    }

    private static void RenderBlazorComponent(RenderTreeBuilder builder, HtmlElement element, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, ref int sequence)
    {
        builder.OpenComponent(sequence++, componentType);

        if (element.Attributes != null)
        {
            var attributes = element.Attributes;

            // Check if componentType is EditForm and add EditContext if so
            if (componentType == typeof(EditForm) && attributes.TryGetValue(nameof(EditForm.Model), out var model))
            {
                var editContext = new EditContext(model);
                builder.AddAttribute(sequence++, nameof(EditContext), editContext);

                // Remove Model attribute from dictionary
                attributes = attributes.Where(x => x.Key != nameof(EditForm.Model)).ToDictionary(x => x.Key, x => x.Value);
            }

            foreach (var attribute in attributes)
            {
                if (attribute.Key.Equals("childcontent", StringComparison.OrdinalIgnoreCase) && element.Children is not null && element.Children.Any())
                {
                    // Create the RenderFragment
                    RenderFragment childContent = builder2 =>
                    {
                        var childSequence = 0;
                        foreach (var child in element.Children)
                        {
                            RenderElement(builder2, child, ref childSequence);
                        }
                    };
                    builder.AddAttribute(sequence++, "ChildContent", childContent);
                }
                else
                {
                    AddAttribute(builder, ref sequence, attribute.Key, attribute.Value, componentType);
                }
            }
        }

        if (element.Children != null && element.Children.Count > 0)
        {
            foreach (var child in element.Children)
            {
                if (child.TagName != null)
                {
                    var property = componentType.GetProperty(child.TagName);
                    if (property != null)
                    {
                        if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(RenderFragment<>))
                        {
                            HandleGenericRenderFragment(builder, ref sequence, child, property, element);
                        }
                        else if (property.PropertyType == typeof(RenderFragment))
                        {
                            if (child.Children != null && child.Children.Count > 0)
                            {
                                var sequenceChild = sequence++;
                                var sequenceChildContent = sequenceChild;

                                // Create the RenderFragment
                                RenderFragment renderFragment = builderChild =>
                                {
                                    foreach (var childChild in child.Children)
                                    {
                                        RenderElement(builderChild, childChild, ref sequenceChild);
                                    }
                                };
                                builder.AddAttribute(sequenceChildContent, child.TagName, renderFragment);
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(child.Content))
                                {
                                    var childContentRender = child.Content.ToRenderFragment();
                                    builder.AddAttribute(sequence++, child.TagName, childContentRender);
                                }
                            }
                        }
                        else
                        {
                            RenderElement(builder, child, ref sequence);
                        }
                    }
                    else
                    {
                        RenderElement(builder, child, ref sequence);
                    }
                }
                else
                {
                    RenderElement(builder, child, ref sequence);
                }
            }
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(element.Content))
            {
                var childContent = element.Content.ToRenderFragment();
                builder.AddAttribute(sequence++, "ChildContent", childContent);
            }
        }

        builder.CloseComponent();
    }

    private static void HandleGenericRenderFragment(RenderTreeBuilder builder, ref int sequence, HtmlElement child, PropertyInfo property, HtmlElement element)
    {
        if (string.IsNullOrWhiteSpace(child.TagName))
        {
            return;
        }

        if (property.PropertyType == typeof(RenderFragment<EditContext>))
        {
            if (child.Children != null && child.Children.Count > 0 && element.Attributes != null && element.Attributes.TryGetValue("Model", out var model))
            {
                var editContext = new EditContext(model);

                RenderFragment<EditContext> renderFragmentEditContext = context =>
                {
                    return builderChild =>
                    {
                        var sequenceChild = 0;
                        foreach (var childChild in child.Children)
                        {
                            RenderElement(builderChild, childChild, ref sequenceChild);
                        }
                    };
                };

                var temp = renderFragmentEditContext.Invoke(editContext);

                builder.AddAttribute(sequence++, child.TagName, temp);
            }
        }
        else
        {
            var typeOfContext = property.PropertyType.GetGenericArguments()[0];
            var renderFragment = child.Content?.ToRenderFragment(typeOfContext);

            if (renderFragment != null)
            {
                builder.AddAttribute(sequence++, child.TagName, renderFragment);
            }
        }
    }

    private static void AddAttribute(RenderTreeBuilder builder, ref int sequence, string name, string value, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] Type? componentType = null)
    {
        if (componentType != null)
        {
            var property = componentType.GetProperty(name);

            if (property != null)
            {
                if (TryParseAttributeValue(value, property.PropertyType, out var parsedValue))
                {
                    builder.AddAttribute(sequence++, name, parsedValue);
                }
                else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(RenderFragment<>))
                {
                    var typeOfContext = property.PropertyType.GetGenericArguments()[0];
                    var renderFragment = value?.ToRenderFragment(typeOfContext);

                    if (renderFragment != null)
                    {
                        builder.AddAttribute(sequence++, name, renderFragment);
                    }
                }
                else if (property.PropertyType == typeof(RenderFragment))
                {
                    var renderFragment = value?.ToRenderFragment();

                    if (renderFragment != null)
                    {
                        builder.AddAttribute(sequence++, name, renderFragment);
                    }
                }
                else
                {
                    builder.AddAttribute(sequence++, name, value);
                }
            }
            else
            {
                builder.AddAttribute(sequence++, name, value);
            }
        }
        else
        {
            builder.AddAttribute(sequence++, name, value);
        }
    }

    /// <summary>
    /// Try to parse the given attribute value to the target type.
    /// </summary>
    /// <param name="value">
    /// The attribute value to parse.
    /// </param>
    /// <param name="targetType">
    /// The target type to parse the attribute value.
    /// </param>
    /// <param name="parsedValue">
    /// The parsed value.
    /// </param>
    /// <returns>
    /// If the attribute value is successfully parsed to the target type, returns true. Otherwise, false.
    /// </returns>
    private static bool TryParseAttributeValue(string? value, Type targetType, out object? parsedValue)
    {
        parsedValue = null;

        if (value == null)
        {
            if (targetType.IsValueType && Nullable.GetUnderlyingType(targetType) == null)
            {
                return false; // Cannot assign null to non-nullable value type
            }
            return true; // null is valid for reference types and nullable value types
        }

        try
        {
            if (targetType == typeof(string))
            {
                parsedValue = value;
                return true;
            }
            if (targetType == typeof(int) && int.TryParse(value, out var intValue))
            {
                parsedValue = intValue;
                return true;
            }
            if (targetType == typeof(bool) && bool.TryParse(value, out var boolValue))
            {
                parsedValue = boolValue;
                return true;
            }
            if (targetType == typeof(double) && double.TryParse(value, out var doubleValue))
            {
                parsedValue = doubleValue;
                return true;
            }
            if (targetType == typeof(decimal) && decimal.TryParse(value, out var decimalValue))
            {
                parsedValue = decimalValue;
                return true;
            }
            if (targetType == typeof(float) && float.TryParse(value, out var floatValue))
            {
                parsedValue = floatValue;
                return true;
            }
            if (targetType == typeof(long) && long.TryParse(value, out var longValue))
            {
                parsedValue = longValue;
                return true;
            }
            if (targetType == typeof(short) && short.TryParse(value, out var shortValue))
            {
                parsedValue = shortValue;
                return true;
            }
            if (targetType == typeof(byte) && byte.TryParse(value, out var byteValue))
            {
                parsedValue = byteValue;
                return true;
            }
            if (targetType == typeof(char) && char.TryParse(value, out var charValue))
            {
                parsedValue = charValue;
                return true;
            }
            if (targetType == typeof(DateTime) && DateTime.TryParse(value, out var dateTimeValue))
            {
                parsedValue = dateTimeValue;
                return true;
            }
            if (targetType == typeof(TimeSpan) && TimeSpan.TryParse(value, out var timeSpanValue))
            {
                parsedValue = timeSpanValue;
                return true;
            }
            if (targetType == typeof(Guid) && Guid.TryParse(value, out var guidValue))
            {
                parsedValue = guidValue;
                return true;
            }
            if (targetType == typeof(Uri))
            {
                parsedValue = new Uri(value, UriKind.RelativeOrAbsolute);
                return true;
            }
            if (targetType == typeof(Version) && Version.TryParse(value, out var versionValue))
            {
                parsedValue = versionValue;
                return true;
            }
            if (targetType.IsEnum && Enum.TryParse(targetType, value, out var enumValue))
            {
                parsedValue = enumValue;
                return true;
            }
        }
        catch
        {
            // Log or handle parsing error
        }

        return false; // If parsing fails
    }
}
