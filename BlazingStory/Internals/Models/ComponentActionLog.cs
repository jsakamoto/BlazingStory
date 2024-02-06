using System.Text.Json;

namespace BlazingStory.Internals.Models;

public class ComponentActionLog
{
    internal readonly string Id = Guid.NewGuid().ToString();

    internal readonly string Name;

    internal readonly string ArgsJson;

    internal int Repeat = 1;

    internal readonly JsonElement ArgsJsonElement;

    internal ComponentActionLog(string name, string argsJson)
    {
        this.Name = name;
        this.ArgsJson = argsJson;
        this.ArgsJsonElement = JsonDocument.Parse(argsJson).RootElement;
    }
}
