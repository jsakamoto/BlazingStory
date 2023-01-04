using BlazingStory.Types;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Models;

internal class StoryContainer
{
    public Type ComponentType { get; }

    public string Title { get; }

    public List<Story> Stories { get; } = new();

    public StoryContainer(Type componentType, string? title)
    {
        if (title == null) throw new ArgumentNullException(nameof(title));
        this.ComponentType = componentType;
        this.Title = title;
    }

    public void RegisterStory(string name, StoryContext storyContext, RenderFragment<StoryContext> renderFragment)
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