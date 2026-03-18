using System.Collections;

namespace BlazingStory.Addons;

/// <summary>
/// A dictionary-like container for global arguments shared across addon toolbar components.
/// </summary>
public class GlobalArguments : IEnumerable<KeyValuePair<string, object?>>
{
    private readonly Dictionary<string, object?> _arguments = new();

    internal event EventHandler? ArgumentsChanged;

    /// <summary>
    /// Gets or sets the argument value associated with the given name, raising <see cref="ArgumentsChanged"/> on change.
    /// </summary>
    /// <param name="name">The argument name.</param>
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

    /// <summary>
    /// Returns an enumerator that iterates over all argument name-value pairs.
    /// </summary>
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => this._arguments.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
