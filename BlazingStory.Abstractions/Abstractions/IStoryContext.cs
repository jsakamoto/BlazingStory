using System.ComponentModel;

namespace BlazingStory.Abstractions;

/// <summary>
/// Provides access to a story's arguments, parameters, and state management.
/// </summary>
public interface IStoryContext
{
    /// <summary>
    /// Gets the current argument values keyed by parameter name.
    /// </summary>
    IReadOnlyDictionary<string, object?> Args { get; }

    /// <summary>
    /// Gets the component parameters associated with this story.
    /// </summary>
    IEnumerable<IComponentParameter> Parameters { get; }

    /// <summary>
    /// Occurs when an argument value is changed.
    /// </summary>
    event AsyncEventHandler? ArgumentChanged;

    /// <summary>
    /// Occurs when argument values are all reset to their default values.
    /// </summary>
    event AsyncEventHandler? ArgumentsReset;

    /// <summary>
    /// This event is used to notify the story that it should re-render.
    /// </summary>
    event EventHandler? ShouldRender;

    /// <summary>
    /// Get the number of parameters that are not event parameters.
    /// </summary>
    int GetNoEventParameterCount();

    /// <summary>
    /// Initializes an argument with the specified name and value.
    /// </summary>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The initial value.</param>
    void InitArgument(string name, object? value);

    /// <summary>
    /// Resets all arguments to their default values.
    /// </summary>
    ValueTask ResetArgumentsAsync();

    /// <summary>
    /// Adds a new argument or updates an existing one with the specified value.
    /// </summary>
    /// <param name="name">The parameter name.</param>
    /// <param name="newValue">The new value to set.</param>
    ValueTask AddOrUpdateArgumentAsync(string name, object? newValue);

    [Obsolete("This method is no longer used and will be removed in a future version."), EditorBrowsable(EditorBrowsableState.Never)]
    string ConvertParameterValueToString(string name, object? value);

    /// <summary>
    /// This method is used to notify the story that it should re-render.
    /// </summary>
    void InvokeShouldRender();
}
