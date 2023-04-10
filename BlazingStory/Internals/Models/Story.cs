using BlazingStory.Types;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Models;

public class Story
{
    internal readonly string Title;

    internal readonly string Name;

    /// <summary>
    /// Gets a navigation path string for this story.<br/>
    /// (ex. "examples-ui-button--primary")
    /// </summary>
    internal readonly string NavigationPath;

    internal readonly StoryContext Context;

    internal readonly RenderFragment<StoryContext> RenderFragment;

    internal Story(string title, string name, StoryContext context, RenderFragment<StoryContext> renderFragment)
    {
        this.Title = title;
        this.Name = name;
        this.Context = context;
        this.RenderFragment = renderFragment;
        this.NavigationPath = Services.Navigation.NavigationPath.Create(this.Title, this.Name);
    }
}