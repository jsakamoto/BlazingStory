using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Utils;

public class JsonFallbackConverter<[DynamicallyAccessedMembers(PublicProperties)] T> : JsonConverter<T>
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotSupportedException();

    private readonly List<object> _visited = new();

    public JsonFallbackConverter()
    {
    }

    public JsonFallbackConverter(List<object> visited)
    {
        this._visited = visited;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        if (value.GetType().IsClass) this._visited.Add(value);

        writer.WriteStartObject();

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            writer.WritePropertyName(property.Name);
            var propValue = property.GetValue(value);
            var isUserDefinedClass = TypeUtility.IsUserDefinedReferenceType(property.PropertyType);

            if (propValue == null)
            {
                writer.WriteNullValue();
            }
            else if (isUserDefinedClass && this._visited.Any(visited => Object.ReferenceEquals(visited, propValue)))
            {
                writer.WriteStringValue("Serialization of cyclic references is not supported.");
            }
            else if (IsUnsupportedType(property.PropertyType))
            {
#pragma warning disable IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
                writer.WriteStringValue($"Serialization of type '{TypeUtility.GetTypeDisplayText(property.PropertyType).First()}' is not supported.");
#pragma warning restore IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
            }
            else
            {
                try
                {
                    if (isUserDefinedClass)
                    {
#pragma warning disable IL2075, IL2076
                        // Create JsonFallbackConverter<T> instance. The type parameter T comes from the property type.
                        var jsonConverterType = typeof(JsonFallbackConverter<>).MakeGenericType(property.PropertyType);
#pragma warning restore IL2075, IL2076
                        if (!options.Converters.Any(converter => converter.GetType().FullName == jsonConverterType.FullName))
                        {
                            var jsonSerializer = (JsonConverter?)Activator.CreateInstance(jsonConverterType, this._visited);
                            options = new JsonSerializerOptions(options);
                            options.Converters.Add(jsonSerializer!);
                        }
                    }
                    JsonSerializer.Serialize(writer, propValue, property.PropertyType, options);
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
    }

    private static bool IsUnsupportedType(Type type)
    {
        return typeof(Delegate).IsAssignableFrom(type) ||
               typeof(System.Linq.Expressions.Expression).IsAssignableFrom(type);
    }
}
