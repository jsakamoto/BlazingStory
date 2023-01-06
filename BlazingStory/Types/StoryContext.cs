using BlazingStory.Internals.Models;

namespace BlazingStory.Types;

public class StoryContext
{
    private readonly Dictionary<string, object?> _DefaultArgs = new();

    private readonly Dictionary<string, object?> _Args = new();

    public IReadOnlyDictionary<string, object?> Args => this._Args;

    internal readonly IEnumerable<ComponentParameter> Parameters;

    internal event AsyncEventHandler? ArgumentChanged;

    internal StoryContext(IEnumerable<ComponentParameter> parameters)
    {
        this.Parameters = parameters;
    }

    internal void InitArgument(string name, object? value)
    {
        this._DefaultArgs[name] = value;
        this._Args[name] = value;
    }

    internal async ValueTask ResetArgumentsAsync()
    {
        this._Args.Clear();
        foreach (var arg in this._DefaultArgs) this._Args.Add(arg.Key, arg.Value);
        await this.ArgumentChanged.InvokeAsync();
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
