namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents a custom page with its full rendered HTML content.
/// </summary>
/// <param name="Title">The title of the custom page.</param>
/// <param name="HtmlContent">The rendered HTML content of the custom page.</param>
public record CustomPageDetail(string Title, string HtmlContent);
