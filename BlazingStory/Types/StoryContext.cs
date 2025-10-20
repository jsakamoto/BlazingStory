using BlazingStory.Internals.Models;
using BlazingStory.Internals.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Types;

public class StoryContext
{
    public IReadOnlyDictionary<string, object?> Args => this._Args;

    internal readonly IEnumerable<ComponentParameter> Parameters;

    private readonly Dictionary<string, object?> _DefaultArgs = new();

    private readonly Dictionary<string, object?> _Args = new();

    internal StoryContext(IEnumerable<ComponentParameter> parameters)
    {
        this.Parameters = parameters;
    }

    internal event AsyncEventHandler? ArgumentChanged;

    /// <summary>
    /// This event is used to notify the story that it should re-render.
    /// </summary>
    internal event EventHandler? ShouldRender;

    /// <summary>
    /// Get the number of parameters that are not event parameters.
    /// </summary>
    internal int GetNoEventParameterCount()
    {
        return this.Parameters
            .Select(p => p.GetParameterTypeStrings().FirstOrDefault())
            .Where(typeString => (typeString != nameof(EventCallback)) && (typeString?.StartsWith(nameof(EventCallback) + "<") == false))
            .Count();
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

    internal async ValueTask ResetAndUpdateArgumentsAsync(IReadOnlyDictionary<string, object?> newArgs)
    {
        // Reset and update in one atomic operation to avoid intermediate re-renders
        this._Args.Clear();
        foreach (var param in this.Parameters.Where(p => p.DefaultValue is not null))
        {
            this._Args[param.Name] = param.DefaultValue;
        }
        foreach (var arg in this._DefaultArgs)
        {
            this._Args[arg.Key] = arg.Value;
        }
        // Apply new args, overwriting defaults
        foreach (var kvp in newArgs)
        {
            this._Args[kvp.Key] = kvp.Value;
        }

        // Trigger ArgumentChanged only once at the end
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

    internal async ValueTask UpdateArgumentsAsync(IReadOnlyDictionary<string, object?> newArgs)
    {
        var changed = false;

        foreach (var kvp in newArgs)
        {
            if (this._Args.TryGetValue(kvp.Key, out var value))
            {
                if ((value == null && kvp.Value == null) || (value != null && value.Equals(kvp.Value)))
                {
                    continue;
                }
                this._Args[kvp.Key] = kvp.Value;
                changed = true;
            }
            else
            {
                this._Args.Add(kvp.Key, kvp.Value);
                changed = true;
            }
        }

        if (changed)
            await this.ArgumentChanged.InvokeAsync();
    }

    /// <summary>
    /// This method is used to notify the story that it should re-render.
    /// </summary>
    internal void InvokeShouldRender()
    {
        this.ShouldRender?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Convert the given parameter value to a string. <br /> If the value is an instance of <see
    /// cref="RenderFragment" /> or <see cref="RenderFragment&lt;T&gt;" />, this method returns the
    /// string that will be rendered by that render fragment. <br /> If the value is an instance of
    /// <see cref="Nullable&lt;T&gt;" />, this method returns "(null)" if the value is null.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter.
    /// </param>
    /// <param name="value">
    /// The value of the parameter.
    /// </param>
    /// <returns>
    /// The string representation of the parameter value.
    /// </returns>
    internal string ConvertParameterValueToString(string name, object? value)
    {
        // EventCallbacks should NEVER be serialized to strings
        if (value != null && value.GetType().Name.StartsWith("EventCallback"))
        {
            return value.ToString() ?? string.Empty;
        }

        if (RenderFragmentKit.TryToString(value, out var str))
        {
            return str;
        }

        if (this.Parameters.TryGetByName(name, out var param) && param.TypeStructure.IsNullable && value == null)
        {
            return "(null)";
        }

        // Serialize arrays and objects as JSON
        if (value != null && (value.GetType().IsArray || value.GetType().IsClass && value.GetType() != typeof(string)))
        {
            try
            {
                return System.Text.Json.JsonSerializer.Serialize(value);
            }
            catch
            {
                // Fall back to ToString() if serialization fails
            }
        }

        return value?.ToString() ?? string.Empty;
    }
}
