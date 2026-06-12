
namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents the result of a component summaries query.
/// </summary>
public class ComponentSummariesResult
{
    /// <summary>
    /// Gets the collection of component summaries.
    /// </summary>
    public IEnumerable<ComponentSummary> Components { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentSummariesResult"/> class with the specified component summaries.
    /// </summary>
    /// <param name="components">The collection of component summaries to include in the result.</param>
    public ComponentSummariesResult(IEnumerable<ComponentSummary> components)
    {
        this.Components = components;
    }
}
