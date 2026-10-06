namespace BlazingStory.Services;

/// <summary>
/// Logs manual story actions to the Blazing Story Actions panel.
/// </summary>
/// <remarks>
/// Blazing Story provides an instance as a cascading value, so a component receives it through a
/// <c>[CascadingParameter]</c> property of this type. The value is <see langword="null"/> when the component runs outside of Blazing Story.
/// </remarks>
public interface IBlazingStoryActionLogger
{
    /// <summary>
    /// Logs an action with an optional payload.
    /// </summary>
    /// <param name="actionName">The action name shown in the Actions panel.</param>
    /// <param name="payload">Optional action payload to serialize and display.</param>
    ValueTask LogAsync(string actionName, object? payload = null);
}
