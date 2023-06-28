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
    private static (bool IsNullable, bool IsGeneric, Type PrimaryType, Type[] SecondayTypes) ExtractTypeStructure(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return (IsNullable: true, IsGeneric: false, PrimaryType: type.GetGenericArguments().First(), SecondayTypes: Array.Empty<Type>());
        }
        else if (type.IsGenericType)
        {
            return (IsNullable: false, IsGeneric: true, PrimaryType: type, SecondayTypes: type.GetGenericArguments());
        }
        else return (IsNullable: false, IsGeneric: false, PrimaryType: type, SecondayTypes: Array.Empty<Type>());
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
}
