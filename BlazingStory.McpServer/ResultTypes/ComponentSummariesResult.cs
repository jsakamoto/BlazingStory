
namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents the result of a component summaries query, including error information if applicable.
/// </summary>
public class ComponentSummariesResult : IErrorResult
{
    /// <summary>
    /// Gets the collection of component summaries.
    /// </summary>
    public IEnumerable<ComponentSummary> Components { get; } = [];

    /// <summary>
    /// Gets a value indicating whether this result represents an error state.
    /// </summary>
    public bool IsError { get; } = false;

    /// <summary>
    /// Gets the error content as a collection of <see cref="TextContent"/> items.
    /// </summary>
    public IEnumerable<TextContent> Content { get; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentSummariesResult"/> class with the specified component summaries.
    /// </summary>
    /// <param name="components">The collection of component summaries to include in the result.</param>
    public ComponentSummariesResult(IEnumerable<ComponentSummary> components)
    {
        this.Components = components;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentSummariesResult"/> class as an error result with the specified error message.
    /// </summary>
    /// <param name="error">A value indicating whether this result is an error.</param>
    /// <param name="message">The error message to include in the result.</param>
    public ComponentSummariesResult(bool error, string message)
    {
        this.IsError = error;
        this.Content = new[] { new TextContent(message) };
    }
}
