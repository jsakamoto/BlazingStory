namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents a summary of a Blazor component with its essential metadata.
/// </summary>
/// <param name="ComponentName">The name of the component.</param>
/// <param name="ComponentType">The full name of the component type, including its namespace.</param>
/// <param name="Summary">A brief description of the component's purpose and functionality.</param>
public record ComponentSummary(string ComponentName, string ComponentType, string Summary);
