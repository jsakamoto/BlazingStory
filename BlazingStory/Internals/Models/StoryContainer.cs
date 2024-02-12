using BlazingStory.Internals.Services.XmlDocComment;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Models;

/// <summary>
/// Represents a "component", container for stories.
/// </summary>
internal class StoryContainer
{
    internal readonly Type TargetComponentType;

    internal readonly string Title;

    internal MarkupString Summary { get; private set; } = default;

    internal readonly List<Story> Stories = new();

    /// <summary>
    /// Gets a navigation path string for this story container (component).<br/>
    /// (ex. "examples-ui-button")
    /// </summary>
    internal readonly string NavigationPath;

    /// <summary>
    /// The type of the layout component to use when displaying these stories.
    /// </summary>
    internal readonly Type? Layout;

    private readonly StoriesRazorDescriptor _StoriesRazorDescriptor;

    private readonly IXmlDocComment _XmlDocComment;

    /// <summary>
    /// Initialize a new instance of <see cref="StoryContainer"/>.
    /// </summary>
    /// <param name="componentType">A type of target UI component in this stories</param>
    /// <param name="layout">A type of the layout component to use when displaying these stories.</param>
    /// <param name="storiesRazorDescriptor">A descriptor of a type of Stories Razor component (..stories.razor) and its <see cref="StoriesAttribute"/>.</param>
    /// <param name="services">A service provider for getting a <see cref="IXmlDocComment"/> service.</param>
    public StoryContainer(Type componentType, Type? layout, StoriesRazorDescriptor storiesRazorDescriptor, IServiceProvider services)
    {
        this._StoriesRazorDescriptor = storiesRazorDescriptor ?? throw new ArgumentNullException(nameof(storiesRazorDescriptor));
        this.TargetComponentType = componentType;
        this.Layout = layout;
        this.Title = this._StoriesRazorDescriptor.StoriesAttribute.Title ?? throw new ArgumentNullException(nameof(storiesRazorDescriptor)); ;
        this.NavigationPath = Services.Navigation.NavigationPath.Create(this.Title);
        this._XmlDocComment = services.GetRequiredService<IXmlDocComment>();
    }

    internal void RegisterStory(string name, StoryContext storyContext, Type? storiesLayout, Type? storyLayout, RenderFragment<StoryContext> renderFragment)
    {
        var newStory = new Story(this._StoriesRazorDescriptor, name, storyContext, storiesLayout, storyLayout, renderFragment);
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

    /// <summary>
    /// Update summary property text of this parameter by reading a XML document comment file.
    /// </summary>
    internal async ValueTask UpdateSummaryFromXmlDocCommentAsync()
    {
        if (this.TargetComponentType == null) return;
        this.Summary = await this._XmlDocComment.GetSummaryOfTypeAsync(this.TargetComponentType);
    }

}