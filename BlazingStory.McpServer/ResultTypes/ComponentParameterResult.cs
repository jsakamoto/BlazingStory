namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents the result of a component parameter operation.
/// </summary>
/// <param name="Success">Indicates whether the operation was successful. Defaults to true.</param>
/// <param name="ErrorMessage">Error message if the operation failed. Null if operation was successful.</param>
/// <param name="Parameters">Collection of component parameters. Empty by default.</param>
public record ComponentParameterResult(
    IEnumerable<ComponentParameter> Parameters,
    bool Success = true,
    string? ErrorMessage = null
);
