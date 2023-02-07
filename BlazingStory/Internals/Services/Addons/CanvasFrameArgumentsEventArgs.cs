
namespace BlazingStory.Internals.Services.Addons;

internal class CanvasFrameArgumentsEventArgs : EventArgs
{
    public IEnumerable<(string Key, object? Value)> Arguments { get; }

    public CanvasFrameArgumentsEventArgs(IEnumerable<(string Key, object? Value)> arguments)
    {
        this.Arguments = arguments;
    }
}
