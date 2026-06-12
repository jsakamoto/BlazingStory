using System.ComponentModel;
using BlazingStory.Abstractions;
using BlazingStory.Internals.Models;
using BlazingStory.ToolKit.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Types;

internal class StoryContext : IStoryContext
{
    private readonly Dictionary<string, object?> _DefaultArgs = new();

    private readonly Dictionary<string, object?> _Args = new();

    public IReadOnlyDictionary<string, object?> Args => this._Args;

    public IEnumerable<IComponentParameter> Parameters { get; }

    public event AsyncEventHandler? ArgumentChanged;

    public event AsyncEventHandler? ArgumentsReset;

    /// <summary>
    /// This event is used to notify the story that it should re-render.
    /// </summary>
    public event EventHandler? ShouldRender;

    internal StoryContext(IEnumerable<ComponentParameter> parameters)
    {
        this.Parameters = parameters;
    }

    /// <summary>
    /// Get the number of parameters that are not event parameters.
    /// </summary>
    public int GetNoEventParameterCount()
    {
        return this.Parameters
            .Select(p => p.GetParameterTypeStrings().FirstOrDefault())
            .Where(typeString => (typeString != nameof(EventCallback)) && (typeString?.StartsWith(nameof(EventCallback) + "<") == false))
            .Count();
    }

    public void InitArgument(string name, object? value)
    {
        this._DefaultArgs[name] = value;
        this._Args[name] = value;
    }

    public async ValueTask ResetArgumentsAsync()
    {
        this._Args.Clear();
        await this.ArgumentsReset.InvokeAsync();

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

    public async ValueTask AddOrUpdateArgumentAsync(string name, object? newValue)
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
        {
            await this.ArgumentChanged.InvokeAsync();
        }
    }

    /// <summary>
    /// This method is used to notify the story that it should re-render.
    /// </summary>
    public void InvokeShouldRender()
    {
        this.ShouldRender?.Invoke(this, EventArgs.Empty);
    }

    [Obsolete("This method is no longer used and will be removed in a future version."), EditorBrowsable(EditorBrowsableState.Never)]
    public string ConvertParameterValueToString(string name, object? value)
    {
        if (value != null && value.GetType().Name.StartsWith("EventCallback"))
        {
            return value.ToString() ?? string.Empty;
        }

        if (RenderFragmentKit.TryToString(value, out var str))
        {
            return str;
        }

        var parameter = this.Parameters.FirstOrDefault(p => p.Name == name);
        if (parameter?.TypeStructure.IsNullable == true && value == null)
        {
            return "(null)";
        }

        if (value != null && (value.GetType().IsArray || (value.GetType().IsClass && value.GetType() != typeof(string))))
        {
            try
            {
                return System.Text.Json.JsonSerializer.Serialize(value);
            }
            catch
            {
            }
        }

        return value?.ToString() ?? string.Empty;
    }
}
