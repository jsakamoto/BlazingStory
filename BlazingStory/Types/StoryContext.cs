using System.ComponentModel;
using BlazingStory.Abstractions;
using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Types;

internal class StoryContext : IStoryContext
{
    private readonly Dictionary<string, object?> _DefaultArgs = new();

    private readonly Dictionary<string, object?> _Args = new();

    public IReadOnlyDictionary<string, object?> Args => this._Args;

    public IEnumerable<IComponentParameter> Parameters { get; }

    public event AsyncEventHandler? ArgumentChanged;

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

    /// <summary>
    /// This method is used to notify the story that it should re-render.
    /// </summary>
    public void InvokeShouldRender()
    {
        this.ShouldRender?.Invoke(this, EventArgs.Empty);
    }

    [Obsolete("This method is no longer used and will be removed in a future version."), EditorBrowsable(EditorBrowsableState.Never)]
    public string ConvertParameterValueToString(string name, object? value) => throw new NotImplementedException();
}
