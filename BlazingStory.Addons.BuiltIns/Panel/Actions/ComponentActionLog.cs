using System.Text.Json;

namespace BlazingStory.Addons.BuiltIns.Panel.Actions;

/// <summary>
/// Represents a single logged entry in the Actions panel for a component event callback invocation.
/// </summary>
public class ComponentActionLog
{
    /// <summary>
    /// The unique identifier for this log entry.
    /// </summary>
    internal readonly string Id = Guid.NewGuid().ToString();

    /// <summary>
    /// The name of the event callback that was invoked.
    /// </summary>
    internal readonly string Name;

    /// <summary>
    /// The raw JSON string of the event arguments, or "void" for parameterless callbacks.
    /// </summary>
    internal readonly string ArgsJson;

    /// <summary>
    /// The number of consecutive times this same action has been logged.
    /// </summary>
    internal int Repeat = 1;

    /// <summary>
    /// The parsed <see cref="JsonElement"/> of the event arguments for display purposes.
    /// </summary>
    internal readonly JsonElement ArgsJsonElement;

    /// <summary>
    /// Initializes a new instance of <see cref="ComponentActionLog"/> with the given name and JSON arguments.
    /// </summary>
    /// <param name="name">The name of the invoked event callback.</param>
    /// <param name="argsJson">The JSON-serialized arguments, or "void" for parameterless callbacks.</param>
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
