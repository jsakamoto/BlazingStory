using BlazingStory.Internals.Models;

namespace BlazingStory.Types;

public class StoryContext
{
    public Dictionary<string, object?> Args { get; } = new();

    internal readonly IEnumerable<ComponentParameter> Parameters;

    internal StoryContext(IEnumerable<ComponentParameter> parameters)
    {
        this.Parameters = parameters;
    }
}
