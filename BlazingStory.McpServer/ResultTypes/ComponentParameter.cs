namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents metadata about a component parameter.
/// </summary>
/// <param name="Name">The name of the component parameter.</param>
/// <param name="Type">The data type name of the parameter.</param>
/// <param name="Required">Indicates whether the parameter is required.</param>
/// <param name="Summary">A description of the parameter's purpose.</param>
/// <param name="ParameterOptions">Available options for the parameter, if applicable.</param>
public record ComponentParameter(
    string Name,
    string Type,
    bool Required,
    string Summary,
    IEnumerable<string> ParameterOptions
);
