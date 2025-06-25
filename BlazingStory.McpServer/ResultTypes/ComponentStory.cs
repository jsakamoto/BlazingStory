namespace BlazingStory.McpServer.ResultTypes;

/// <summary>
/// Represents a story for a UI component in BlazingStory, similar to Storybook stories.
/// A story showcases a component in a specific state or configuration.
/// </summary>
/// <param name="Name">The unique identifier/name for the story.</param>
/// <param name="Title">The display title of the story shown in the UI.</param>
/// <param name="Description">A description of the story, explaining the component state or configuration being demonstrated.</param>
/// <param name="CodeSnippet">The code snippet that demonstrates how to use the component in this specific story.</param>
public record ComponentStory(
    string Name,
    string Title,
    string Description,
    string CodeSnippet
);
