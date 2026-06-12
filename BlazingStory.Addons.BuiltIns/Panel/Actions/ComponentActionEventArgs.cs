namespace BlazingStory.Addons.BuiltIns.Panel.Actions;

/// <summary>
/// Holds the data for a component action event captured in the Actions panel.
/// </summary>
public class ComponentActionEventArgs
{
    /// <summary>
    /// Gets the name of the event callback that was invoked.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets the JSON-serialized representation of the event arguments.
    /// </summary>
    public string? ArgsJson { get; init; }
}
