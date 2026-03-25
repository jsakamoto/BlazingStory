
namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents the result of retrieving component stories from the server.
/// </summary>
public class ComponentStoriesResult : IErrorResult
{
    /// <summary>
    /// Gets the collection of component stories retrieved from the server.
    /// </summary>
    /// <value>
    /// An enumerable collection of <see cref="ComponentStory"/> objects. 
    /// Returns an empty collection if no stories are available or if an error occurred.
    /// </value>
    public IEnumerable<ComponentStory> Stories { get; } = [];

    /// <summary>
    /// Gets a value indicating whether this result represents an error state.
    /// </summary>
    /// <value>
    /// <c>true</c> if the operation failed and this result contains error information; 
    /// otherwise, <c>false</c> if the operation succeeded.
    /// </value>
    public bool IsError { get; } = false;

    /// <summary>
    /// Gets the error content as a collection of text content items.
    /// </summary>
    /// <value>
    /// A collection of <see cref="TextContent"/> objects containing error details.
    /// Returns an empty collection when <see cref="IsError"/> is <c>false</c>.
    /// </value>
    public IEnumerable<TextContent> Content { get; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentStoriesResult"/> class with successful stories.
    /// </summary>
    /// <param name="Stories">The collection of component stories that were successfully retrieved.</param>
    public ComponentStoriesResult(IEnumerable<ComponentStory> Stories)
    {
        this.Stories = Stories;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentStoriesResult"/> class with error information.
    /// </summary>
    /// <param name="error">A value indicating whether this result represents an error state.</param>
    /// <param name="message">The error message to include in the result content.</param>
    public ComponentStoriesResult(bool error, string message)
    {
        this.IsError = error;
        this.Content = new[] { new TextContent(message) };
    }
}
