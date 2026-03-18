using Microsoft.Extensions.Logging;

namespace BlazingStory.ToolKit.Extensions;

/// <summary>
/// Extension methods for <see cref="ValueTask"/>.
/// </summary>
public static class ValueTaskExtensions
{
    /// <summary>
    /// Registers a continuation on the <see cref="ValueTask"/> that logs any exception to the given logger if the task faults.
    /// </summary>
    /// <param name="task">The task to observe.</param>
    /// <param name="logger">The logger to write the error to.</param>
    public static void AndLogException(this ValueTask task, ILogger logger)
    {
        var awaiter = task.GetAwaiter();
        awaiter.OnCompleted(() =>
        {
            if (!task.IsFaulted) return;
            try { awaiter.GetResult(); }
            catch (Exception e) { logger.LogError(e, message: e.Message); }
        });
    }
}
