using BlazingStory.Internals.Models;

namespace BlazingStory.Types;

public class StoryContext
{
    private readonly Dictionary<string, object?> _DefaultArgs = new();

    private readonly Dictionary<string, object?> _Args = new();

    public IReadOnlyDictionary<string, object?> Args => this._Args;

    internal readonly IEnumerable<ComponentParameter> Parameters;

    internal event AsyncEventHandler? ArgumentChanged;

    /// <summary>
    /// This event is used to notify the story that it should re-render.
    /// </summary>
    internal event EventHandler? ShouldRender;

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
        foreach (var param in this.Parameters.Where(p => p.DefaultValue is not null))
        {
            this._Args[param.Name] = param.DefaultValue;
        }
        foreach (var arg in this._DefaultArgs)
        {
            this._Args[arg.Key] = arg.Value;
        }
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

    /// <summary>
    /// This method is used to notify the story that it should re-render.
    /// </summary>
    internal void InvokeShouldRender()
    {
        this.ShouldRender?.Invoke(this, EventArgs.Empty);
    }
}
