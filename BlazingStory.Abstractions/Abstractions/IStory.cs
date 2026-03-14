namespace BlazingStory.Abstractions;

public interface IStory
{
    string Title { get; }

    string Name { get; }

    /// <summary>
    /// The type of the target UI component in this story.
    /// </summary>
    Type ComponentType { get; }

    StoriesRazorDescriptor StoriesRazorDescriptor { get; }

    IStoryContext Context { get; }

    /// <summary>
    /// Gets a navigation path string for this story.<br/>
    /// (ex. "examples-ui-button--primary")
    /// </summary>
    string NavigationPath { get; }
}
