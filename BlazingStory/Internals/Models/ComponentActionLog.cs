using System.Text.Json;

namespace BlazingStory.Internals.Models;

public class ComponentActionLog
{
    internal readonly string Id = Guid.NewGuid().ToString();

    internal readonly string Name;

    internal readonly string ArgsJson;

    internal readonly JsonElement ArgsJsonElement;
    internal int Repeat = 1;

    internal ComponentActionLog(string name, string argsJson)
    {
        this.Name = name;
        this.ArgsJson = argsJson;

        if (argsJson != "void")
        {
            this.ArgsJsonElement = JsonDocument.Parse(argsJson).RootElement;
        }
    }
}
