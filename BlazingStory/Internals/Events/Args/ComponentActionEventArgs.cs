namespace BlazingStory.Internals.Events.Args;

public class ComponentActionEventArgs : EventArgs
{
    public string? Name { get; init; }

    public string? ArgsJson { get; init; }
}
