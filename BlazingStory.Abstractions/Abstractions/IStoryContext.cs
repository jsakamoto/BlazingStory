namespace BlazingStory.Abstractions;

public interface IStoryContext
{
    IReadOnlyDictionary<string, object?> Args { get; }

    IEnumerable<IComponentParameter> Parameters { get; }

    event AsyncEventHandler? ArgumentChanged;

    /// <summary>
    /// Get the number of parameters that are not event parameters.
    /// </summary>
    int GetNoEventParameterCount();

    void InitArgument(string name, object? value);

    ValueTask ResetArgumentsAsync();

    ValueTask AddOrUpdateArgumentAsync(string name, object? newValue);

    string ConvertParameterValueToString(string name, object? value);
}
