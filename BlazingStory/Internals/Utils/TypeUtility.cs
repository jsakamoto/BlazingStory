using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazingStory.Internals.Utils;

internal static class TypeUtility
{
    /// <summary>
    /// Returns the name of the given type as a C# language keyword.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static IEnumerable<string> GetTypeDisplayText(Type type)
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
    internal static TypeStructure ExtractTypeStructure(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return new(isNullable: true, isGeneric: false, primaryType: type.GetGenericArguments().First(), secondaryTypes: Array.Empty<Type>());
        }
        else if (type.IsGenericType)
        {
            return new(isNullable: false, isGeneric: true, primaryType: type, secondaryTypes: type.GetGenericArguments());
        }
        else return new(isNullable: false, isGeneric: false, primaryType: type, secondaryTypes: Array.Empty<Type>());
    }

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

    /// <summary>
    /// Try to convert the given string to the given type.<br/>
    /// (Most cases, this method uses for deserialize URL query parameters of iframe to component parameters.)
    /// </summary>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="targetTypeStructure">The structure of the type to convert to.</param>
    /// <param name="sourceString">The string to convert from.</param>
    /// <param name="convertedValue">The converted value if the conversion is successful.</param>
    /// <returns>True if the conversion is successful, otherwise false.</returns>
    internal static bool TryConvertType(Type targetType, TypeStructure targetTypeStructure, string sourceString, out object? convertedValue)
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

        else if (primaryType == typeof(int))
        {
            if (int.TryParse(sourceString, out var numValue))
            {
                convertedValue = numValue;
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
            RenderFragment renderFragment = (RenderTreeBuilder builder) => builder.AddContent(0, sourceString);
            convertedValue = renderFragment;
            return true;
        }

        else if (primaryType.IsGenericTypeOf(typeof(RenderFragment<>)))
        {
            var argumentType = primaryType.GetGenericArguments().First();
            convertedValue = RenderFragmentKit.FromString(argumentType, sourceString);
            return true;
        }

        convertedValue = null;
        return false;
    }
}
