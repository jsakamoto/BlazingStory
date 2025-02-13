using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Utils;
public class RenderFragmentJsonConverter : JsonConverter<RenderFragment>
{
    public override RenderFragment Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException("Deserialization of RenderFragment is not supported.");
    }

    public override void Write(Utf8JsonWriter writer, RenderFragment value, JsonSerializerOptions options)
    {
        writer.WriteStringValue("RenderFragment serialization is not supported.");
    }
}