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
}