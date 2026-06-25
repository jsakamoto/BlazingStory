using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.JavaScriptAPI;

internal class BlazingStoryAPI
{
    private readonly NavigationService _NavigationService;

    private StoryIndex? _StoryIndex;

    public BlazingStoryAPI(IServiceProvider services)
    {
        this._NavigationService = services.GetRequiredService<NavigationService>();
    }

    [JSInvokable(nameof(GetStoryIndex))]
    public StoryIndex GetStoryIndex()
    {
        this._StoryIndex ??= this.ConvertToIndex();
        return this._StoryIndex;
    }

    private StoryIndex ConvertToIndex()
    {
        var entries = this._NavigationService.Root.EnumAll()
            .Where(item => item.Type is NavigationItemType.Story or NavigationItemType.Docs or NavigationItemType.CustomPage)
            .Select(ToStoryIndexEntry)
            .ToDictionary(entry => entry.Id, entry => entry);

        return new StoryIndex { V = 1, Entries = entries };
    }

    private static StoryIndexEntry ToStoryIndexEntry(NavigationTreeItem item)
    {
        var id = item.NavigationPath.Split('/', 3)[^1];
        var (title, name, type) = item.Type switch
        {
            NavigationItemType.Story => (string.Join("/", item.PathSegments), item.Caption, "story"),
            NavigationItemType.Docs => (string.Join("/", item.PathSegments), item.Caption, "docs"),
            NavigationItemType.CustomPage => (string.Join("/", item.PathSegments.Append(item.Caption)), item.Caption, "docs"),
            _ => throw new InvalidOperationException($"Unexpected NavigationItemType: {item.Type}")
        };
        return new StoryIndexEntry { Id = id, Title = title, Name = name, Type = type };
    }
}
