using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Services.Navigation;

internal class NavigationService
{
    private readonly NavigationManager _NavigationManager;

    private readonly NavigationHistory _NavigationHistory;

    private NavigationTreeItem _Root = new();

    private int _SearchResultSequence = 0;

    public NavigationService(NavigationManager navigationManager, HelperScript helperScript)
    {
        this._NavigationManager = navigationManager;
        this._NavigationHistory = new(helperScript);
    }

    internal NavigationTreeItem BuildNavigationTree(IEnumerable<StoryContainer> storyContainers, string? expandedNavigationPath)
    {
        this._Root = new NavigationTreeBuilder().Build(storyContainers, expandedNavigationPath);
        return this._Root;
    }

    internal string GetNavigationUrl(INavigationPath item) => "./?path=/story/" + item.NavigationPath;

    internal void NavigateTo(INavigationPath item)
    {
        this._NavigationManager.NavigateTo(this.GetNavigationUrl(item));
    }

    internal ValueTask<IEnumerable<NavigationListItem>> GetHistoryItemsAsync()
    {
        return this._NavigationHistory.GetItemsAsync();
    }

    internal async ValueTask AddHistoryAsync(NavigationTreeItem active)
    {
        await this._NavigationHistory.AddAsync(this._Root, active);
    }

    internal async ValueTask ClearHistoryAsync()
    {
        await this._NavigationHistory.ClearAsync();
    }

    internal IEnumerable<NavigationListItem> Search(IEnumerable<string>? keywords)
    {
        if (keywords == null || keywords.Where(word => !string.IsNullOrEmpty(word)).Any() == false) return Enumerable.Empty<NavigationListItem>();

        var results = new List<NavigationListItem>();

        foreach (var item in this._Root.EnumAll())
        {
            if (item.Type == NavigationTreeItemType.Story && keywords.Any(word => item.Caption.Contains(word, StringComparison.InvariantCultureIgnoreCase)))
            {
                results.Add(NavigationListItem.CreateFrom(this._SearchResultSequence++, item));
            }
        }

        // TODO: Re-oreder the search results

        return results;
    }
}
