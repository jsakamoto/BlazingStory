
namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents the result of retrieving component stories from the server.
/// </summary>
public class ComponentStoriesResult
{
    /// <summary>
    /// Gets the collection of component stories retrieved from the server.
    /// </summary>
    /// <value>
    /// An enumerable collection of <see cref="ComponentStory"/> objects. 
    /// Returns an empty collection if no stories are available.
    /// </value>
    public IEnumerable<ComponentStory> Stories { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentStoriesResult"/> class with successful stories.
    /// </summary>
    /// <param name="Stories">The collection of component stories that were successfully retrieved.</param>
    public ComponentStoriesResult(IEnumerable<ComponentStory> Stories)
    {
        this.Stories = Stories;
    }
}
