namespace BlazingStory.Abstractions;

/// <summary>
/// Represents a single story definition for a UI component.
/// </summary>
public interface IStory
{
    /// <summary>
    /// Gets the display title of this story.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Gets the name of this story.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The type of the target UI component in this story.
    /// </summary>
    Type ComponentType { get; }

    /// <summary>
    /// Gets the descriptor of the Stories Razor component that defines this story.
    /// </summary>
    StoriesRazorDescriptor StoriesRazorDescriptor { get; }

    /// <summary>
    /// Gets the story context containing arguments and parameters for this story.
    /// </summary>
    IStoryContext Context { get; }

    /// <summary>
    /// Gets a navigation path string for this story.<br/>
    /// (ex. "examples-ui-button--primary")
    /// </summary>
    string NavigationPath { get; }
}
