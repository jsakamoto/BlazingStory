namespace BlazingStory.Abstractions;

/// <summary>
/// Represents an asynchronous event handler that returns a <see cref="ValueTask"/>.
/// </summary>
public delegate ValueTask AsyncEventHandler();

/// <summary>
/// Provides extension methods for <see cref="AsyncEventHandler"/>.
/// </summary>
public static class AsyncEventHandlerExtensions
{
    /// <summary>
    /// Invokes all delegates in the invocation list sequentially and awaits each one.
    /// </summary>
    /// <param name="handler">The event handler to invoke.</param>
    public static async ValueTask InvokeAsync(this AsyncEventHandler? handler)
    {
        if (handler == null) return;
        foreach (var invocation in handler.GetInvocationList())
        {
            await (ValueTask)(invocation.Method.Invoke(invocation.Target, Array.Empty<object>()) ?? ValueTask.CompletedTask);
        }
    }
}