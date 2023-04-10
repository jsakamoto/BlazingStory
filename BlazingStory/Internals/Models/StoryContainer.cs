using BlazingStory.Types;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Models;

/// <summary>
/// Represents a "component", container for stories.
/// </summary>
internal class StoryContainer
{
    internal readonly Type ComponentType;

    internal readonly string Title;

    internal readonly List<Story> Stories = new();

    /// <summary>
    /// Gets a navigation path string for this story container (component).<br/>
    /// (ex. "examples-ui-button")
    /// </summary>
    internal readonly string NavigationPath;

    /// <summary>
    /// Initialize a new instance of <see cref="StoryContainer"/>.
    /// </summary>
    /// <param name="componentType">A type of Razor component</param>
    /// <param name="title">A title of this container ("component")</param>
    public StoryContainer(Type componentType, string? title)
    {
        if (title == null) throw new ArgumentNullException(nameof(title));
        this.ComponentType = componentType;
        this.Title = title;
        this.NavigationPath = Services.Navigation.NavigationPath.Create(this.Title);
    }

    internal void RegisterStory(string name, StoryContext storyContext, RenderFragment<StoryContext> renderFragment)
    {
        var newStory = new Story(this.Title, name, storyContext, renderFragment);
        var index = this.Stories.FindIndex(story => story.Name == name);
        if (index == -1)
        {
            this.Stories.Add(newStory);
        }
        else
        {
            var story = this.Stories[index];
            if (Object.ReferenceEquals(story.RenderFragment, renderFragment) == false)
            {
                this.Stories[index] = newStory;
            }
        }
    }
}