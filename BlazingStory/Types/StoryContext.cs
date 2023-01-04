using BlazingStory.Internals.Models;

namespace BlazingStory.Types;

public class StoryContext
{
    private readonly Dictionary<string, object?> _Args = new();

    public IReadOnlyDictionary<string, object?> Args => this._Args;

    internal readonly IEnumerable<ComponentParameter> Parameters;

    internal event AsyncEventHandler? ArgumentChanged;

    internal StoryContext(IEnumerable<ComponentParameter> parameters)
    {
        this.Parameters = parameters;
    }

    internal async ValueTask AddOrUpdateArgumentAsync(string name, object? newValue)
    {
        if (this._Args.TryGetValue(name, out var value))
        {
            if ((value == null && newValue == null) || (value != null && value.Equals(newValue))) return;
            this._Args[name] = newValue;
        }
        else
        {
            this._Args.Add(name, newValue);
        }

        await this.ArgumentChanged.InvokeAsync();
    }
}
