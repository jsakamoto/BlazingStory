namespace BlazingStory.Abstractions;

public delegate ValueTask AsyncEventHandler();

public static class AsyncEventHandlerExtensions
{
    public static async ValueTask InvokeAsync(this AsyncEventHandler? handler)
    {
        if (handler == null) return;
        foreach (var invocation in handler.GetInvocationList())
        {
            await (ValueTask)(invocation.Method.Invoke(invocation.Target, Array.Empty<object>()) ?? ValueTask.CompletedTask);
        }
    }
}