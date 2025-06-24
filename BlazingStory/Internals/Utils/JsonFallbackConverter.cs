using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Utils;

public class JsonFallbackConverter<[DynamicallyAccessedMembers(PublicProperties)] T> : JsonConverter<T>
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotSupportedException();

    private readonly Stack<object> _visited;

    public JsonFallbackConverter(Stack<object>? visited = null)
    {
        this._visited = visited ?? new();
    }

    private const string _fallbackMessageFormat = "Serialization of {0} is not supported.";

#pragma warning disable IL2026, IL2027, IL2072
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var typeCode = value is null ? TypeCode.Empty : Type.GetTypeCode(typeof(T));

        // Serialize null-thy values
        if (value is null || typeCode is TypeCode.Empty or TypeCode.DBNull)
        {
            writer.WriteNullValue();
            return;
        }

        // Serialize primitive types directly
        switch (value)
        {
            case DateTime d:
                writer.WriteRawValue(d.ToUniversalTime().ToString("o"), skipInputValidation: true);
                return;
            case DateTimeOffset d:
                writer.WriteRawValue(d.UtcDateTime.ToString("o"), skipInputValidation: true);
                return;
            case DateOnly d:
                writer.WriteRawValue(d.ToString("o"), skipInputValidation: true);
                return;
            case TimeOnly t:
                writer.WriteRawValue(t.ToString("o"), skipInputValidation: true);
                return;
            default: // Handle other primitive types, like strings, numbers, etc.
                if (typeCode is not TypeCode.Object)
                {
                    JsonSerializer.Serialize(writer, value, typeof(T));
                    return;
                }
                break;
        }

        // Handle cyclic references and unsupported types
        var isClass = value.GetType().IsClass;
        if (isClass)
        {
            if (this._visited.Any(visited => Object.ReferenceEquals(visited, value)))
            {
                writer.WriteStringValue(string.Format(_fallbackMessageFormat, "cyclic references"));
                return;
            }
            this._visited.Push(value);
        }

        // Serialize properties of the object
        writer.WriteStartObject();

        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0);

        foreach (var property in properties)
        {
            writer.WritePropertyName(property.Name);

            var propValue = property.GetValue(value);
            var propType = propValue?.GetType() ?? property.PropertyType;

            if (IsUnsupportedType(propType))
            {
                writer.WriteStringValue(string.Format(_fallbackMessageFormat, $"type '{TypeUtility.GetTypeDisplayText(propType).First()}'"));
            }
            else if (propValue is IDictionary dictionary)
            {
                writer.WriteStartObject();
                foreach (DictionaryEntry entry in dictionary)
                {
                    writer.WritePropertyName(entry.Key.ToString() ?? "");
                    var valueType = entry.Value?.GetType() ?? typeof(object);
                    JsonSerializer.Serialize(writer, entry.Value, valueType, this.PrepareJsonSerializerOptions(options, valueType));
                }
                writer.WriteEndObject();
            }
            else if (propValue is IEnumerable enumerable && propValue is not string)
            {
                writer.WriteStartArray();
                foreach (var item in enumerable)
                {
                    var valueType = item?.GetType() ?? typeof(object);
                    JsonSerializer.Serialize(writer, item, valueType, this.PrepareJsonSerializerOptions(options, valueType));
                }
                writer.WriteEndArray();
            }
            else
            {
                try
                {
                    JsonSerializer.Serialize(writer, propValue, propType, this.PrepareJsonSerializerOptions(options, propType));
                }
                catch (Exception)
                {
                    writer.WriteStringValue(string.Format(_fallbackMessageFormat, $"type '{TypeUtility.GetTypeDisplayText(propType).First()}'"));
                }
            }
        }

        writer.WriteEndObject();
        if (isClass) this._visited.Pop();
    }
#pragma warning restore IL2026, IL2027, IL2072

    private JsonSerializerOptions PrepareJsonSerializerOptions(JsonSerializerOptions options, Type valueType)
    {
        if (TypeUtility.IsPlainObjectType(valueType))
        {
#pragma warning disable IL2070, IL2071
            var jsonConverterType = typeof(JsonFallbackConverter<>).MakeGenericType(valueType);
#pragma warning restore IL2070, IL2071
            if (!options.Converters.Any(converter => converter.GetType().FullName == jsonConverterType.FullName))
            {
                var jsonSerializer = (JsonConverter?)Activator.CreateInstance(jsonConverterType, this._visited);
                options = new JsonSerializerOptions(options);
                options.Converters.Add(jsonSerializer!);
            }
        }
        return options;
    }

    private static bool IsUnsupportedType(Type type)
    {
        return type == typeof(EventCallback) ||
                TypeUtility.GetOpenType(type) == typeof(EventCallback<>) ||
                typeof(Delegate).IsAssignableFrom(type) ||
                typeof(Expression).IsAssignableFrom(type);
    }
}
