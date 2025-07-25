namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents a result that can contain error information.
/// </summary>
public interface IErrorResult
{
    /// <summary>
    /// Gets a value indicating whether this result represents an error state.
    /// </summary>
    /// <value>
    /// <c>true</c> if this result is an error; otherwise, <c>false</c>.
    /// </value>
    bool IsError { get; }

    /// <summary>
    /// Gets the error content as a collection of text content items.
    /// </summary>
    /// <value>
    /// A collection of <see cref="TextContent"/> objects containing error details.
    /// </value>
    IEnumerable<TextContent> Content { get; }
}
