namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents text content with an optional type specification.
/// </summary>
public class TextContent
{
    /// <summary>
    /// Gets or sets the type of the content. The default is "text".
    /// </summary>
    public string Type { get; set; } = "text";

    /// <summary>
    /// Gets or sets the text content.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextContent"/> class.
    /// </summary>
    public TextContent(string text)
    {
        this.Text = text;
    }
}
