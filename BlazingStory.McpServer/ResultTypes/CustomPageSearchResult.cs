namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents the result of searching custom pages.
/// </summary>
public class CustomPageSearchResult
{
    /// <summary>
    /// Gets the collection of search hits, ordered by relevance.
    /// </summary>
    public IEnumerable<CustomPageSearchHit> Results { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomPageSearchResult"/> class.
    /// </summary>
    /// <param name="results">The search hits ordered by relevance.</param>
    public CustomPageSearchResult(IEnumerable<CustomPageSearchHit> results)
    {
        this.Results = results;
    }
}
