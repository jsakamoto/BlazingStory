using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Navigation;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.JavaScriptAPI;

internal class BlazingStoryAPI
{
    private readonly StoriesStore _StorieStore;

    private StoryIndex? _StoryIndex;

    public BlazingStoryAPI(StoriesStore storiesStore)
    {
        this._StorieStore = storiesStore;
    }

    [JSInvokable(nameof(GetStoryIndex))]
    public StoryIndex GetStoryIndex()
    {
        this._StoryIndex ??= this.ConvertStoriesStoreToIndex();
        return this._StoryIndex;
    }

    private StoryIndex ConvertStoriesStoreToIndex()
    {
        var entries = this._StorieStore
            .EnumAllStories()
            .GroupBy(story => story.Title)
            .SelectMany(g =>
                g.Select(story => new StoryIndexEntry
                {
                    Id = story.NavigationPath,
                    Title = story.Title,
                    Name = story.Name,
                    Type = "story"
                }).Prepend(new StoryIndexEntry
                {
                    Id = NavigationPath.Create(g.Key, "docs"),
                    Title = g.Key,
                    Name = "Docs",
                    Type = "docs"
                })
            )
            .ToDictionary(entry => entry.Id, entry => entry);

        return new StoryIndex
        {
            V = 1,
            Entries = entries
        };
    }
}
