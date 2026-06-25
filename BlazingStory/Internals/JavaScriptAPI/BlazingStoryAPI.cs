using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.JavaScriptAPI;

/// <summary>
/// Provides JavaScript-invokable methods that expose Blazing Story internals to the browser.
/// </summary>
internal class BlazingStoryAPI
{
    private readonly NavigationService _NavigationService;

    private StoryIndex? _StoryIndex;

    /// <summary>
    /// Initializes a new instance of <see cref="BlazingStoryAPI"/> by resolving <see cref="NavigationService"/> from the service provider.
    /// </summary>
    /// <param name="services">The application service provider.</param>
    public BlazingStoryAPI(IServiceProvider services)
    {
        this._NavigationService = services.GetRequiredService<NavigationService>();
    }

    /// <summary>
    /// Returns a Storybook-compatible Story Index built from the current navigation tree, cached after the first call.
    /// </summary>
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

    /// <summary>
    /// Converts a navigation tree item into a <see cref="StoryIndexEntry"/>.
    /// </summary>
    /// <param name="item">The navigation tree item to convert.</param>
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
