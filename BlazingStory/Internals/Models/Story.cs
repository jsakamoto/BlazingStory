using BlazingStory.Types;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Models;

public class Story
{
    internal readonly StoriesRazorDescriptor StoriesRazorDescriptor;

    /// <summary>
    /// The type of the target UI component in this story.
    /// </summary>
    internal readonly Type ComponentType;

    internal readonly string Title;

    internal readonly string Name;

    /// <summary>
    /// Gets a navigation path string for this story.<br/>
    /// (ex. "examples-ui-button--primary")
    /// </summary>
    internal readonly string NavigationPath;

    internal readonly StoryContext Context;

    internal readonly Type? StoriesLayout;

    internal readonly Type? StoryLayout;

    internal readonly RenderFragment<StoryContext> RenderFragment;

    internal Story(StoriesRazorDescriptor storiesRazorDescriptor, Type componentType, string name, StoryContext context, Type? storiesLayout, Type? storyLayout, RenderFragment<StoryContext> renderFragment)
    {
        this.StoriesRazorDescriptor = storiesRazorDescriptor ?? throw new ArgumentNullException(nameof(storiesRazorDescriptor));
        this.Title = this.StoriesRazorDescriptor.StoriesAttribute.Title ?? throw new ArgumentNullException(nameof(storiesRazorDescriptor));
        this.ComponentType = componentType;
        this.Name = name;
        this.Context = context;
        this.StoriesLayout = storiesLayout;
        this.StoryLayout = storyLayout;
        this.RenderFragment = renderFragment;
        this.NavigationPath = Services.Navigation.NavigationPath.Create(this.Title, this.Name);
    }
}