namespace BlazingStory.Internals.JavaScriptAPI;

internal class StoryIndexEntry
{
    /// <summary>
    /// e.g."example-button--primary"
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// e.g. "Example/Button"
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// e.g. "Primary"
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// e.g. "docs", "story"
    /// </summary>
    public required string Type { get; init; }
}
