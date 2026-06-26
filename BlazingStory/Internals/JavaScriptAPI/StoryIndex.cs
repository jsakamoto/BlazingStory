namespace BlazingStory.Internals.JavaScriptAPI;

internal class StoryIndex
{
    public required int V { get; init; }

    public required Dictionary<string, StoryIndexEntry> Entries { get; init; }
}
