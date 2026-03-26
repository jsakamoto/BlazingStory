namespace BlazingStory.Internals.Events.Args;

internal class IntersectionChangeEventArgs : EventArgs
{
    public bool IsIntersecting { get; init; }
}
