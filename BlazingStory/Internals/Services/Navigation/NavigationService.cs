using System.Diagnostics.CodeAnalysis;
using BlazingStory.Internals.Models;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Services.Navigation;

internal class NavigationService
{
    private readonly NavigationManager _NavigationManager;

    private readonly NavigationHistory _NavigationHistory;

    private NavigationTreeItem _Root = new();

    private NavigationTreeItem? _LastNavigated = null;

    private bool _LastNavigatedWasunknown = false;

    private int _SearchResultSequence = 0;

    public NavigationService(NavigationManager navigationManager, HelperScript helperScript)
    {
        this._NavigationManager = navigationManager;
        this._NavigationHistory = new(helperScript);
    }

    internal NavigationTreeItem BuildNavigationTree(IEnumerable<StoryContainer> storyContainers, IEnumerable<CustomPageContainer> customPageContainers, IList<NavigationTreeOrdering>? customOrderings, string? expandedNavigationPath)
    {
        this._Root = new NavigationTreeBuilder().Build(storyContainers, customPageContainers, customOrderings, expandedNavigationPath);
        return this._Root;
    }

    internal string GetNavigationUrl(INavigationPath item) => "./?path=" + item.NavigationPath;

    internal void NavigateTo(INavigationPath item)
    {
        this._NavigationManager.NavigateTo(this.GetNavigationUrl(item));
    }

    internal void NavigateToDefaultStory(QueryRouteData? routeData)
    {
        if (this.TryGetActiveNavigationItem(routeData, out var _, out var storyItems)) return;

        var firstStory = storyItems.FirstOrDefault();
        if (firstStory == null) return;

        this._Root.EnsureExpandedTo(firstStory);
        this.NavigateTo(firstStory);
    }

    internal bool TryGetActiveNavigationItem(QueryRouteData? routeData, [NotNullWhen(true)] out NavigationTreeItem? activeItem, out IEnumerable<NavigationTreeItem> navigatableItems)
    {
        activeItem = null;
        navigatableItems = this._Root.EnumAll()
            .Where(item => item.Type is NavigationItemType.Story or NavigationItemType.Docs or NavigationItemType.CustomPage)
            .ToArray();

        var navigationPath = routeData?.Path;
        if (string.IsNullOrEmpty(navigationPath)) return false;

        activeItem = navigatableItems.FirstOrDefault(item => item.NavigationPath == navigationPath);
        if (activeItem == null) return false;

        return true;
    }

    internal void NavigateToNextComponentItem(QueryRouteData? routeData, bool navigateToNext)
    {
        if (!this.TryGetActiveNavigationItem(routeData, out var activeItem, out var _)) return;
        var allComponents = this._Root.EnumAll().Where(item => item.Type is NavigationItemType.Component).ToList();
        var ativeComponentIndex = allComponents.FindIndex(item => item.EnumAll().Contains(activeItem));
        var nextComponentIndex = ativeComponentIndex + (navigateToNext ? +1 : -1);
        if (nextComponentIndex < 0 || allComponents.Count <= nextComponentIndex) return;
        var nextComponent = allComponents[nextComponentIndex];
        var nextItem = nextComponent.EnumAll().Where(item => item.Type is NavigationItemType.Story or NavigationItemType.Docs or NavigationItemType.CustomPage).FirstOrDefault();
        if (nextItem == null) return;
        this.NavigateTo(nextItem);
    }

    internal void NavigateToNextDocsOrStory(QueryRouteData? routeData, bool navigateToNext)
    {
        if (!this.TryGetActiveNavigationItem(routeData, out var activeItem, out var _)) return;
        var allItems = this._Root.EnumAll().Where(item => item.Type is NavigationItemType.Docs or NavigationItemType.Story or NavigationItemType.CustomPage).ToList();
        var ativeIndex = allItems.FindIndex(item => item == activeItem);
        var nextIndex = ativeIndex + (navigateToNext ? +1 : -1);
        if (nextIndex < 0 || allItems.Count <= nextIndex) return;
        var nextItem = allItems[nextIndex];
        this.NavigateTo(allItems[nextIndex]);
    }

    internal void NotifyLastVisitedWasUnknown()
    {
        this._LastNavigatedWasunknown = true;
    }

    internal void BackToLastNavigated()
    {
        if (!this._LastNavigatedWasunknown)
        {
            if (this._LastNavigated != null)
            {
                this.NavigateTo(this._LastNavigated);
            }
            else
            {
                this.NavigateToDefaultStory(null);
            }
        }
    }

    internal ValueTask<IEnumerable<NavigationListItem>> GetHistoryItemsAsync()
    {
        return this._NavigationHistory.GetItemsAsync();
    }

    internal async ValueTask AddHistoryAsync(NavigationTreeItem active)
    {
        this._LastNavigated = active;
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
        this.SearchCore(this._Root, keywords, results);
        return results;
    }

    private void SearchCore(NavigationTreeItem item, IEnumerable<string> keywords, List<NavigationListItem> results)
    {
        if (item.Type is NavigationItemType.Component or NavigationItemType.Docs or NavigationItemType.Story or NavigationItemType.CustomPage)
        {
            if (keywords.Any(word => item.Caption.Contains(word, StringComparison.InvariantCultureIgnoreCase)))
            {
                results.Add(NavigationListItem.CreateFrom(this._SearchResultSequence++, item));
                return;
            }
        }

        foreach (var subItem in item.SubItems)
        {
            this.SearchCore(subItem, keywords, results);
        }
    }
}
