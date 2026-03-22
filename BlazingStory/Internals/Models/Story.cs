using System.Diagnostics.CodeAnalysis;
using BlazingStory.Abstractions;
using Microsoft.AspNetCore.Components;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Models;

internal class Story : IStory
{
    public StoriesRazorDescriptor StoriesRazorDescriptor { get; }

    /// <summary>
    /// The type of the target UI component in this story.
    /// </summary>
    public Type ComponentType { get; }

    public string Title { get; }

    public string Name { get; }

    /// <summary>
    /// Gets a navigation path string for this story.<br/>
    /// (ex. "examples-ui-button--primary")
    /// </summary>
    public string NavigationPath { get; }

    public IStoryContext Context { get; }

    [DynamicallyAccessedMembers(All)]
    internal readonly Type? StoriesLayout;

    [DynamicallyAccessedMembers(All)]
    internal readonly Type? StoryLayout;

    internal readonly RenderFragment<IStoryContext> RenderFragment;

    internal readonly RenderFragment? Description;

    internal Story(StoriesRazorDescriptor storiesRazorDescriptor, Type componentType, string name, IStoryContext context, [DynamicallyAccessedMembers(All)] Type? storiesLayout, [DynamicallyAccessedMembers(All)] Type? storyLayout, RenderFragment<IStoryContext> renderFragment, RenderFragment? description)
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
        this.Description = description;
    }
}