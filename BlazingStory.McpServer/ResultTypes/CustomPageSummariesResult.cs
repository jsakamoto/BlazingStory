namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents the result of a custom page summaries query.
/// </summary>
public class CustomPageSummariesResult
{
    /// <summary>
    /// Gets the collection of custom page summaries.
    /// </summary>
    public IEnumerable<CustomPageSummary> CustomPages { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomPageSummariesResult"/> class with the specified custom page summaries.
    /// </summary>
    /// <param name="customPages">The collection of custom page summaries to include in the result.</param>
    public CustomPageSummariesResult(IEnumerable<CustomPageSummary> customPages)
    {
        this.CustomPages = customPages;
    }
}
