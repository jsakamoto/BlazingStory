using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BlazingStory.Abstractions;
using BlazingStory.ToolKit.Extensions;
using Microsoft.AspNetCore.Components;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.ToolKit.Utils;

/// <summary>
/// Utility class for type inspection and conversion operations.
/// </summary>
public static class TypeUtility
{
    /// <summary>
    /// Returns the display text of the given type as a C# language keyword, including enum values if applicable.
    /// </summary>
    /// <param name="type">The type to get the display text for.</param>
    public static IEnumerable<string> GetTypeDisplayText([DynamicallyAccessedMembers(PublicConstructors | PublicMethods | Interfaces)] Type type)
    {
        var (isNullable, isGeneric, primaryType, secondaryTypes) = TypeUtility.ExtractTypeStructure(type);
        if (primaryType.IsEnum)
        {
            yield return primaryType.Name + (isNullable ? "?" : "");
            foreach (var enumValue in Enum.GetValues(primaryType))
            {
                yield return $"\"{enumValue}\"";
            }
        }
        else if (isGeneric)
        {
#pragma warning disable IL2067 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
            var typeArguments = string.Join(", ", secondaryTypes.SelectMany(t => GetTypeDisplayText(t)));
#pragma warning restore IL2067 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
            yield return GetTypeNameAsLangKeyword(primaryType) + "<" + typeArguments + ">" + (isNullable ? "?" : "");
        }
        else
        {
            yield return GetTypeNameAsLangKeyword(primaryType) + (isNullable ? "?" : "");
        }
    }

    /// <summary>
    /// Extracts type structure of the given type.
    /// </summary>
    public static TypeStructure ExtractTypeStructure([DynamicallyAccessedMembers(PublicConstructors | PublicMethods | Interfaces)] Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
#pragma warning disable IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
            return new(isNullable: true, isGeneric: false, primaryType: type.GetGenericArguments().First(), secondaryTypes: []);
#pragma warning restore IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
        }
        else if (type.IsGenericType)
        {
            return new(isNullable: false, isGeneric: true, primaryType: type, secondaryTypes: type.GetGenericArguments());
        }
        else return new(isNullable: false, isGeneric: false, primaryType: type, secondaryTypes: []);
    }

    /// <summary>
    /// Get name of the type as a C# language keyword.
    /// </summary>
    public static string GetTypeNameAsLangKeyword(Type type)
    {
        return Type.GetTypeCode(type) switch
        {
            TypeCode.Boolean => "bool",
            TypeCode.Byte => "byte",
            TypeCode.Char => "char",
            TypeCode.Decimal => "decimal",
            TypeCode.Double => "double",
            TypeCode.Int16 => "short",
            TypeCode.Int32 => "int",
            TypeCode.Int64 => "long",
            TypeCode.SByte => "sbyte",
            TypeCode.Single => "float",
            TypeCode.String => "string",
            TypeCode.UInt16 => "ushort",
            TypeCode.UInt32 => "uint",
            TypeCode.UInt64 => "ulong",
            _ => type.Name.Split('`').First()
        };
    }

    /// <summary>
    /// Try to convert the given string to the given type.<br/>
    /// (Most cases, this method uses for deserialize URL query parameters of iframe to component parameters.)
    /// </summary>
    /// <param name="targetTypeStructure">The structure of the type to convert to.</param>
    /// <param name="sourceString">The string to convert from.</param>
    /// <param name="convertedValue">The converted value if the conversion is successful.</param>
    /// <returns>True if the conversion is successful, otherwise false.</returns>
    public static bool TryConvertType(TypeStructure targetTypeStructure, string sourceString, out object? convertedValue) => TryConvertType(targetTypeStructure, sourceString, out convertedValue, formatProvider: null);

    /// <summary>
    /// Try to convert the given string to the given type.<br/>
    /// (Most cases, this method uses for deserialize URL query parameters of iframe to component parameters.)
    /// </summary>
    /// <param name="targetTypeStructure">The structure of the type to convert to.</param>
    /// <param name="sourceString">The string to convert from.</param>
    /// <param name="convertedValue">The converted value if the conversion is successful.</param>
    /// <param name="formatProvider">The format provider to use for parsing, if applicable.</param>
    /// <returns>True if the conversion is successful, otherwise false.</returns>
    public static bool TryConvertType(TypeStructure targetTypeStructure, string sourceString, out object? convertedValue, IFormatProvider? formatProvider)
    {
        var primaryType = targetTypeStructure.PrimaryType;
        var isNullable = targetTypeStructure.IsNullable;

        if (isNullable && sourceString == "(null)")
        {
            convertedValue = null;
            return true;
        }

        else if (primaryType == typeof(string))
        {
            convertedValue = sourceString;
            return true;
        }

        else if (primaryType == typeof(bool))
        {
            if (bool.TryParse(sourceString, out var boolValue))
            {
                convertedValue = boolValue;
                return true;
            }
        }

        else if (primaryType.IsEnum)
        {
            if (Enum.TryParse(primaryType, sourceString, out var enumValue))
            {
                convertedValue = enumValue;
                return true;
            }
        }

        else if (primaryType == typeof(RenderFragment))
        {
            convertedValue = sourceString.ToRenderFragment();
            return true;
        }

        else if (primaryType.IsGenericTypeOf(typeof(RenderFragment<>)))
        {
            var argumentType = primaryType.GetGenericArguments().First();
            convertedValue = sourceString.ToRenderFragment(argumentType);
            return true;
        }

        else if (primaryType.IsArray)
        {
            var elementType = primaryType.GetElementType();
            if (elementType is not null)
            {
                convertedValue = ConvertToCollection(
                    sourceString,
                    elementType,
                    createCollection: (size) => Array.CreateInstance(elementType, size),
                    setValue: (array, index, value) => array.SetValue(value, index),
                    formatProvider);
                return true;
            }
            convertedValue = null;
            return false;
        }

        else if (targetTypeStructure.IsGeneric && IsListCompatibleType(primaryType))
        {
            var elementType = targetTypeStructure.SecondaryTypes[0];
            var listType = typeof(List<>).MakeGenericType(elementType);

            convertedValue = ConvertToCollection(
                sourceString,
                elementType,
                createCollection: (size) => (IList)Activator.CreateInstance(listType)!,
                setValue: (list, index, value) => list.Add(value),
                formatProvider);
            return true;
        }

        else if (IsParsableType(primaryType))
        {
            var tryParseMethod = primaryType.GetMethod(nameof(IParsable<int>.TryParse), BindingFlags.Public | BindingFlags.Static, [typeof(string), typeof(IFormatProvider), primaryType.MakeByRefType()]);
            var parameters = new object?[] { sourceString, formatProvider, Activator.CreateInstance(primaryType) };
            if ((bool)(tryParseMethod?.Invoke(null, parameters) ?? false))
            {
                convertedValue = parameters[2];
                return true;
            }
        }

        convertedValue = null;
        return false;
    }

    private static TCollection ConvertToCollection<TCollection>(string sourceString, Type elementType, Func<int, TCollection> createCollection, Action<TCollection, int, object?> setValue, IFormatProvider? formatProvider)
    {
        var items = sourceString.Split(',')
            .Select(s => s.Replace("%2c", ",").Replace("%25", "%"))
            .ToArray();
        var collection = createCollection(items.Length);

        for (var i = 0; i < items.Length; i++)
        {
#pragma warning disable IL2067
            var elementStructure = ExtractTypeStructure(elementType);
#pragma warning restore IL2067
            if (TryConvertType(elementStructure, items[i], out var elementValue, formatProvider))
            {
                setValue(collection, i, elementValue);
            }
            else setValue(collection, i, null);
        }

        return collection;
    }

    /// <summary>
    /// Returns whether the given type is <see cref="List{T}"/> or a generic collection interface assignable from <see cref="List{T}"/>,
    /// such as <see cref="IEnumerable{T}"/>, <see cref="ICollection{T}"/>, <see cref="IList{T}"/>,
    /// <see cref="IReadOnlyCollection{T}"/>, or <see cref="IReadOnlyList{T}"/>.
    /// </summary>
    private static bool IsListCompatibleType(Type type)
    {
        if (!type.IsGenericType) return false;
        var openType = type.GetGenericTypeDefinition();
        return openType == typeof(List<>)
            || openType == typeof(IEnumerable<>)
            || openType == typeof(ICollection<>)
            || openType == typeof(IList<>)
            || openType == typeof(IReadOnlyCollection<>)
            || openType == typeof(IReadOnlyList<>);
    }

    /// <summary>
    /// Returns whether the given type implements <see cref="IParsable{TSelf}"/>.
    /// </summary>
    public static bool IsParsableType([DynamicallyAccessedMembers(Interfaces)] Type type)
    {
        return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IParsable<>));
    }

    /// <summary>
    /// Returns whether the given type is a numeric type.
    /// </summary>
    public static bool IsNumericType(Type type) => Type.GetTypeCode(type) switch
    {
        TypeCode.Byte => true,
        TypeCode.SByte => true,
        TypeCode.Int16 => true,
        TypeCode.UInt16 => true,
        TypeCode.Int32 => true,
        TypeCode.UInt32 => true,
        TypeCode.Int64 => true,
        TypeCode.UInt64 => true,
        TypeCode.Single => true,
        TypeCode.Double => true,
        TypeCode.Decimal => true,
        _ => false
    };

    /// <summary>
    /// Returns whether the given type is a decimal point type.
    /// </summary>
    public static bool IsDecimalPointType(Type type) => Type.GetTypeCode(type) switch
    {
        TypeCode.Single => true,
        TypeCode.Double => true,
        TypeCode.Decimal => true,
        _ => false
    };

    /// <summary>
    /// Get open type of the given type.<br/>
    /// When you pass the `List&lt;T&gt;` type, this method returns `List&lt;&gt;`.
    /// </summary>
    public static Type GetOpenType(Type type) => type.IsGenericType ? type.GetGenericTypeDefinition() : type;

    /// <summary>
    /// Returns whether the given object is a plain object type that should be serialized as a JSON object, like '{"foo":123,"bar:456}}'.
    /// </summary>
    public static bool IsPlainObjectType(Type type)
    {
        // Exclude value types (structs)
        if (type.IsValueType) return false;

        // Exclude common built-in types
        if (type == typeof(string) ||
            type.IsArray ||
            typeof(System.Collections.IEnumerable).IsAssignableFrom(type)) // Includes List, Dictionary, etc.
        {
            return false;
        }

        return true;
    }
}
