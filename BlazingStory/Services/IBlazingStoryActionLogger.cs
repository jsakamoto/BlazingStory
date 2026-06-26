namespace BlazingStory.Services;

/// <summary>
/// Logs manual story actions to the BlazingStory Actions panel.
/// </summary>
public interface IBlazingStoryActionLogger
{
    /// <summary>
    /// Logs an action with an optional payload.
    /// </summary>
    /// <param name="actionName">The action name shown in the Actions panel.</param>
    /// <param name="payload">Optional action payload to serialize and display.</param>
    ValueTask LogAsync(string actionName, object? payload = null);
}
