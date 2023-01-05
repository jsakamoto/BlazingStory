using Microsoft.Extensions.Logging;

namespace BlazingStory.Internals.Extensions;

internal static class ValueTaskExtensions
{
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
