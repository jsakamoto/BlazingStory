using System.Diagnostics.CodeAnalysis;
using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Services;

public class StoriesStore
{
    private readonly List<StoryContainer> _StoryContainers = new();

    internal IEnumerable<StoryContainer> StoryContainers => this._StoryContainers;

    internal StoriesStore()
    {
    }

    internal void RegisterStoryContainer(StoryContainer storyContainer)
    {
        var index = this._StoryContainers.FindIndex(container => container.Title == storyContainer.Title);
        if (index != -1)
        {
            if (Object.ReferenceEquals(this._StoryContainers[index], storyContainer) == false)
            {
                this._StoryContainers[index] = storyContainer;
            }
        }
        else
        {
            this._StoryContainers.Add(storyContainer);
        }
    }

    internal IEnumerable<Story> EnumAllStories()
    {
        return this._StoryContainers.SelectMany(container => container.Stories);
    }

    /// <summary>
    /// Try to find story by navigationn path, such as "examples-ui-button--default".
    /// </summary>
    internal bool TryGetStoryByPath(string navigationPath, [NotNullWhen(true)] out Story? story)
    {
        story = this.EnumAllStories().FirstOrDefault(s => s.NavigationPath == navigationPath);
        return story != null;
    }
}