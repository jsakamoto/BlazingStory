namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents the result of a component parameter operation.
/// </summary>
public class ComponentParameterResult
{
    /// <summary>
    /// Gets the collection of component parameters.
    /// </summary>
    /// <value>
    /// An enumerable collection of <see cref="ComponentParameter"/> objects.
    /// </value>
    public IEnumerable<ComponentParameter> Parameters { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentParameterResult"/> class with the specified parameters.
    /// </summary>
    /// <param name="parameters">The collection of component parameters.</param>
    public ComponentParameterResult(IEnumerable<ComponentParameter> parameters)
    {
        this.Parameters = parameters;
    }
}
