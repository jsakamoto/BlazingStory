namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents a search result for a custom page, including a content snippet.
/// </summary>
/// <param name="Title">The title of the matching custom page.</param>
/// <param name="Snippet">A text snippet showing content around the matched terms.</param>
public record CustomPageSearchHit(string Title, string Snippet);
