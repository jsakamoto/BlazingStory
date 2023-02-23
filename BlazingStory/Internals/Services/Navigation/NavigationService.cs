using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Services.Navigation;

internal class NavigationService
{
    private readonly NavigationManager _NavigationManager;

    private readonly NavigationHistory _NavigationHistory;

    public NavigationService(NavigationManager navigationManager, HelperScript helperScript)
    {
        this._NavigationManager = navigationManager;
        this._NavigationHistory = new(helperScript);
    }

    internal NavigationTreeItem BuildNavigationTree(IEnumerable<StoryContainer> storyContainers, string? expandedNavigationPath)
    {
        return new NavigationTreeBuilder().Build(storyContainers, expandedNavigationPath);
    }

    internal string GetNavigationUrl(INavigationPath item) => "./?path=/story/" + item.NavigationPath;

    internal void NavigateTo(INavigationPath item)
    {
        this._NavigationManager.NavigateTo(this.GetNavigationUrl(item));
    }

    internal ValueTask<IEnumerable<NavigationHistoryItem>> GetHistoryItemsAsync()
    {
        return this._NavigationHistory.GetItemsAsync();
    }

    internal async ValueTask AddHistoryAsync(NavigationTreeItem root, NavigationTreeItem active)
    {
        await this._NavigationHistory.AddAsync(root, active);
    }

    internal async ValueTask ClearHistoryAsync()
    {
        await this._NavigationHistory.ClearAsync();
    }
}
