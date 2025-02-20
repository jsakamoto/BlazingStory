using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
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

    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        var isClass = value.GetType().IsClass;
        if (isClass)
        {
            if (this._visited.Any(visited => Object.ReferenceEquals(visited, value)))
            {
                writer.WriteStringValue("Serialization of cyclic references is not supported.");
                return;
            }
            this._visited.Push(value);
        }

        writer.WriteStartObject();

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            writer.WritePropertyName(property.Name);

            var propValue = property.GetValue(value);

            if (propValue == null)
            {
                writer.WriteNullValue();
            }
            else if (IsUnsupportedType(property.PropertyType))
            {
#pragma warning disable IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
                writer.WriteStringValue($"Serialization of type '{TypeUtility.GetTypeDisplayText(property.PropertyType).First()}' is not supported.");
#pragma warning restore IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
            }
            else if (propValue is string s)
            {
                writer.WriteStringValue(s);
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
            else if (propValue is IEnumerable enumerable)
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
                    JsonSerializer.Serialize(writer, propValue, property.PropertyType, this.PrepareJsonSerializerOptions(options, property.PropertyType));
                }
                catch (Exception)
                {
#pragma warning disable IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
                    writer.WriteStringValue($"Serialization of type '{TypeUtility.GetTypeDisplayText(property.PropertyType).First()}' is not supported.");
#pragma warning restore IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
                }
            }
        }

        writer.WriteEndObject();
        if (isClass) this._visited.Pop();
    }

    private JsonSerializerOptions PrepareJsonSerializerOptions(JsonSerializerOptions options, Type valueType)
    {
        if (TypeUtility.IsUserDefinedReferenceType(valueType))
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
        return typeof(Delegate).IsAssignableFrom(type) ||
               typeof(Expression).IsAssignableFrom(type);
    }
}
