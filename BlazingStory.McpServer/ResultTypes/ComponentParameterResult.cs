namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents the result of a component parameter operation.
/// </summary>
public class ComponentParameterResult : IErrorResult
{
    /// <summary>
    /// Gets the collection of component parameters.
    /// </summary>
    /// <value>
    /// An enumerable collection of <see cref="ComponentParameter"/> objects. Empty by default.
    /// </value>
    public IEnumerable<ComponentParameter> Parameters { get; } = [];

    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    /// <value>
    /// <c>true</c> if the operation failed; otherwise, <c>false</c>. Defaults to <c>false</c>.
    /// </value>
    public bool IsError { get; } = false;

    /// <summary>
    /// Gets the error content as a collection of text content items.
    /// </summary>
    /// <value>
    /// A collection of <see cref="TextContent"/> objects containing error details. Empty by default.
    /// </value>
    public IEnumerable<TextContent> Content { get; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentParameterResult"/> class with the specified parameters.
    /// </summary>
    /// <param name="parameters">The collection of component parameters.</param>
    public ComponentParameterResult(IEnumerable<ComponentParameter> parameters)
    {
        this.Parameters = parameters;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentParameterResult"/> class with error information.
    /// </summary>
    /// <param name="error">A value indicating whether the operation failed.</param>
    /// <param name="message">The error message.</param>
    public ComponentParameterResult(bool error, string message)
    {
        this.IsError = error;
        this.Content = new[] { new TextContent(message) };
    }
}
