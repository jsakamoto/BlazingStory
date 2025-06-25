namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents the result of retrieving component stories from the server.
/// </summary>
/// <param name="Stories">The collection of component stories retrieved.</param>
/// <param name="Success">Indicates whether the retrieval operation was successful. Defaults to true.</param>
/// <param name="ErrorMessage">Contains error information when Success is false. Null when the operation succeeds.</param>
public record ComponentStoriesResult(
    IEnumerable<ComponentStory> Stories,
    bool Success = true,
    string? ErrorMessage = null
);
