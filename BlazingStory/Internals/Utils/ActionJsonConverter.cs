using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazingStory.Internals.Utils;
public class ActionJsonConverter : JsonConverter<Action>
{
    public override Action Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException("Deserialization of System.Action is not supported.");
    }

    public override void Write(Utf8JsonWriter writer, Action value, JsonSerializerOptions options)
    {
        writer.WriteStringValue("RenderFragment System.Action is not supported.");
    }
}