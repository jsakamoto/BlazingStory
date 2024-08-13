using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using BlazingStory.Internals.Extensions;
using Microsoft.AspNetCore.Components;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Utils;

internal static class TypeUtility
{
    /// <summary>
    /// Returns the name of the given type as a C# language keyword.
    /// </summary>
    /// <param name="type">
    /// The type to get the name of.
    /// </param>
    /// <returns>
    /// The name of the type as a C# language keyword.
    /// </returns>
    [SuppressMessage("Trimming", "IL2067:Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.", Justification = "<Pending>")]
    internal static IEnumerable<string> GetTypeDisplayText([DynamicallyAccessedMembers(PublicConstructors | PublicMethods | Interfaces)] Type type)
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
            var typeArguments = string.Join(", ", secondaryTypes.SelectMany(t => GetTypeDisplayText(t)));

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
    /// <param name="type">
    /// The type to extract the structure from.
    /// </param>
    /// <returns>
    /// The extracted type structure.
    /// </returns>
    [SuppressMessage("Trimming", "IL2072:Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.", Justification = "<Pending>")]
    internal static TypeStructure ExtractTypeStructure([DynamicallyAccessedMembers(PublicConstructors | PublicMethods | Interfaces)] Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return new(isNullable: true, isGeneric: false, primaryType: type.GetGenericArguments().First(), secondaryTypes: Array.Empty<Type>());
        }
        else if (type.IsGenericType)
        {
            return new(isNullable: false, isGeneric: true, primaryType: type, secondaryTypes: type.GetGenericArguments());
        }
        else
        {
            return new(isNullable: false, isGeneric: false, primaryType: type, secondaryTypes: Array.Empty<Type>());
        }
    }

    /// <summary>
    /// Try to convert the given string to the given type. <br /> (Most cases, this method uses for
    /// deserialize URL query parameters of iframe to component parameters.)
    /// </summary>
    /// <param name="targetTypeStructure">
    /// The structure of the type to convert to.
    /// </param>
    /// <param name="sourceString">
    /// The string to convert from.
    /// </param>
    /// <param name="convertedValue">
    /// The converted value if the conversion is successful.
    /// </param>
    /// <returns>
    /// True if the conversion is successful, otherwise false.
    /// </returns>
    internal static bool TryConvertType(this TypeStructure targetTypeStructure, string sourceString, out object? convertedValue)
    {
        var primaryType = targetTypeStructure.PrimaryType;
        var isNullable = targetTypeStructure.IsNullable;

        // Handle nullable types
        if (isNullable && sourceString == "(null)")
        {
            convertedValue = null;
            return true;
        }

        // Handle string type
        if (primaryType == typeof(string))
        {
            convertedValue = sourceString;
            return true;
        }

        // Handle boolean type
        if (primaryType == typeof(bool))
        {
            if (bool.TryParse(sourceString, out var boolValue))
            {
                convertedValue = boolValue;
                return true;
            }
        }

        // Handle enum types
        if (primaryType.IsEnum)
        {
            if (Enum.TryParse(primaryType, sourceString, out var enumValue))
            {
                convertedValue = enumValue;
                return true;
            }
        }

        // Handle RenderFragment
        if (primaryType == typeof(RenderFragment))
        {
            convertedValue = sourceString.ToRenderFragment();
            return true;
        }

        // Handle generic RenderFragment<T>
        if (primaryType.IsGenericTypeOf(typeof(RenderFragment<>)))
        {
            var argumentType = primaryType.GetGenericArguments().First();
            convertedValue = sourceString.ToRenderFragment(argumentType);
            return true;
        }

        // Handle parsable types (numeric types)
        if (IsParsableType(primaryType))
        {
            var numberStyles = NumberStyles.None;

            // Set appropriate NumberStyles based on type
            if (primaryType == typeof(float) || primaryType == typeof(double) || primaryType == typeof(decimal))
            {
                numberStyles = NumberStyles.Float | NumberStyles.AllowThousands;
            }
            else if (primaryType == typeof(int) || primaryType == typeof(long) || primaryType == typeof(short) ||
                     primaryType == typeof(uint) || primaryType == typeof(ulong) || primaryType == typeof(ushort))
            {
                numberStyles = NumberStyles.Integer;
            }

            // Attempt to find and invoke TryParse method with NumberStyles and CultureInfo
            var tryParseMethod = primaryType.GetMethod(nameof(IParsable<int>.TryParse), BindingFlags.Public | BindingFlags.Static, null,
                new[] { typeof(string), typeof(NumberStyles), typeof(IFormatProvider), primaryType.MakeByRefType() }, null);

            if (tryParseMethod != null)
            {
                var parameters = new object?[] { sourceString, numberStyles, CultureInfo.InvariantCulture, Activator.CreateInstance(primaryType) };

                if ((bool)(tryParseMethod.Invoke(null, parameters) ?? false))
                {
                    convertedValue = parameters[3]; // The result is in the last parameter (index 3)
                    return true;
                }
            }
        }

        // Fall-back: conversion failed
        convertedValue = null;
        return false;
    }

    /// <summary>
    /// Returns whether the given type implements <see cref="IParsable{TSelf}" />.
    /// </summary>
    internal static bool IsParsableType([DynamicallyAccessedMembers(Interfaces)] Type type)
    {
        return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IParsable<>));
    }

    /// <summary>
    /// Returns whether the given type is a numeric type.
    /// </summary>
    internal static bool IsNumericType(this Type type) => Type.GetTypeCode(type) switch
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
    internal static bool IsDecimalPointType(Type type) => Type.GetTypeCode(type) switch
    {
        TypeCode.Single => true,
        TypeCode.Double => true,
        TypeCode.Decimal => true,
        _ => false
    };

    /// <summary>
    /// Get open type of the given type. <br /> When you pass the `List&lt;T&gt;` type, this method
    /// returns `List&lt;&gt;`.
    /// </summary>
    internal static Type GetOpenType(Type type) => type.IsGenericType ? type.GetGenericTypeDefinition() : type;

    /// <summary>
    /// Get name of the type as a C# language keyword.
    /// </summary>
    private static string GetTypeNameAsLangKeyword(Type type)
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
}
