namespace BlazingStory.Internals.Events.Args;

/// <summary>
/// Provides data for an event that occurs when the frame height changes.
/// </summary>
internal class FrameHeightChangeEventArgs : EventArgs
{
    /// <summary>Gets or sets the frame height value (px).</summary>
    public int Height { get; init; }
}
