using Microsoft.JSInterop;

namespace BlazingStory.Internals.Extensions;

internal static class IJSExtensions
{
    public static async ValueTask DisposeIfConnectedAsync(this IJSObjectReference? value)
    {
        try
        {
            if (value != null) await value.DisposeAsync();
        }
        catch (JSDisconnectedException) { }
    }
}
