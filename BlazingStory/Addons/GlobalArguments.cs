using System.Collections;

namespace BlazingStory.Addons.Internals;

public class GlobalArguments : IEnumerable<KeyValuePair<string, object?>>
{
    private readonly Dictionary<string, object?> _arguments = new();

    internal event EventHandler? ArgumentsChanged;

    public object? this[string name]
    {
        get => this._arguments.TryGetValue(name, out var value) ? value : null;
        set
        {
            if (this._arguments.TryGetValue(name, out var oldValue) && EqualityComparer<object?>.Default.Equals(oldValue, value)) return;
            this._arguments[name] = value;
            this.ArgumentsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => this._arguments.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
