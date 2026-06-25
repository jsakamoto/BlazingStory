using System.Diagnostics.CodeAnalysis;
using BlazingStory.Internals.Models;
using BlazingStory.Types;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Services.Navigation;

/// <summary>
/// Manages navigation state, history, and search across stories, docs, and custom pages.
/// </summary>
internal class NavigationService
{
    private readonly NavigationManager _NavigationManager;

    private readonly NavigationHistory _NavigationHistory;

    /// <summary>
    /// Gets the root item of the currently built navigation tree.
    /// </summary>
    internal NavigationTreeItem Root { get; private set; } = new();

    private NavigationTreeItem? _LastNavigated = null;

    private bool _LastNavigatedWasunknown = false;

    private int _SearchResultSequence = 0;

    public NavigationService(NavigationManager navigationManager, IJSRuntime jsRuntime)
    {
        this._NavigationManager = navigationManager;
        this._NavigationHistory = new(jsRuntime);
    }

    /// <summary>
    /// Builds the navigation tree from story containers and custom page containers and stores it in <see cref="Root"/>.
    /// </summary>
    /// <param name="storyContainers">Story containers to include in the navigation tree.</param>
    /// <param name="customPageContainers">Custom page containers to include in the navigation tree.</param>
    /// <param name="customOrderings">Custom ordering rules for navigation tree items, or <c>null</c> for default ordering.</param>
    /// <param name="expandedNavigationPath">Navigation path of the item that should be expanded, or <c>null</c> to expand none.</param>
    internal NavigationTreeItem BuildNavigationTree(IEnumerable<StoryContainer> storyContainers, IEnumerable<CustomPageContainer> customPageContainers, IList<NavigationTreeOrderEntry>? customOrderings, string? expandedNavigationPath)
    {
        this.Root = new NavigationTreeBuilder().Build(storyContainers, customPageContainers, customOrderings, expandedNavigationPath);
        return this.Root;
    }

    /// <summary>
    /// Returns the URL for navigating to the specified navigation item.
    /// </summary>
    /// <param name="item">The navigation item to get the URL for.</param>
    internal string GetNavigationUrl(INavigationPath item) => "./?path=" + item.NavigationPath;

    /// <summary>
    /// Navigates the browser to the specified navigation item.
    /// </summary>
    /// <param name="item">The navigation item to navigate to.</param>
    internal void NavigateTo(INavigationPath item)
    {
        this._NavigationManager.NavigateTo(this.GetNavigationUrl(item));
    }

    /// <summary>
    /// Navigates to the first available story if no active navigation item matches the current route.
    /// </summary>
    /// <param name="routeData">The current route data used to determine the active item.</param>
    internal void NavigateToDefaultStory(QueryRouteData? routeData)
    {
        if (this.TryGetActiveNavigationItem(routeData, out var _, out var storyItems)) return;

        var firstStory = storyItems.FirstOrDefault();
        if (firstStory == null) return;

        this.Root.EnsureExpandedTo(firstStory);
        this.NavigateTo(firstStory);
    }

    /// <summary>
    /// Tries to find the active navigation item matching the current route and returns all navigatable items.
    /// </summary>
    /// <param name="routeData">The current route data used to match the active item.</param>
    /// <param name="activeItem">The matched active navigation item, or <c>null</c> if none matched.</param>
    /// <param name="navigatableItems">All navigatable items (stories, docs, and custom pages) in the tree.</param>
    internal bool TryGetActiveNavigationItem(QueryRouteData? routeData, [NotNullWhen(true)] out NavigationTreeItem? activeItem, out IEnumerable<NavigationTreeItem> navigatableItems)
    {
        activeItem = null;
        navigatableItems = this.Root.EnumAll()
            .Where(item => item.Type is NavigationItemType.Story or NavigationItemType.Docs or NavigationItemType.CustomPage)
            .ToArray();

        var navigationPath = routeData?.Path;
        if (string.IsNullOrEmpty(navigationPath)) return false;

        activeItem = navigatableItems.FirstOrDefault(item => item.NavigationPath == navigationPath);
        if (activeItem == null) return false;

        return true;
    }

    /// <summary>
    /// Navigates to the first story or docs item in the next or previous component relative to the currently active item.
    /// </summary>
    /// <param name="routeData">The current route data used to determine the active item.</param>
    /// <param name="navigateToNext"><c>true</c> to navigate to the next component; <c>false</c> for the previous.</param>
    internal void NavigateToNextComponentItem(QueryRouteData? routeData, bool navigateToNext)
    {
        if (!this.TryGetActiveNavigationItem(routeData, out var activeItem, out var _)) return;
        var allComponents = this.Root.EnumAll().Where(item => item.Type is NavigationItemType.Component).ToList();
        var ativeComponentIndex = allComponents.FindIndex(item => item.EnumAll().Contains(activeItem));
        var nextComponentIndex = ativeComponentIndex + (navigateToNext ? +1 : -1);
        if (nextComponentIndex < 0 || allComponents.Count <= nextComponentIndex) return;
        var nextComponent = allComponents[nextComponentIndex];
        var nextItem = nextComponent.EnumAll().Where(item => item.Type is NavigationItemType.Story or NavigationItemType.Docs or NavigationItemType.CustomPage).FirstOrDefault();
        if (nextItem == null) return;
        this.NavigateTo(nextItem);
    }

    /// <summary>
    /// Navigates to the next or previous docs, story, or custom page item relative to the currently active item.
    /// </summary>
    /// <param name="routeData">The current route data used to determine the active item.</param>
    /// <param name="navigateToNext"><c>true</c> to navigate to the next item; <c>false</c> for the previous.</param>
    internal void NavigateToNextDocsOrStory(QueryRouteData? routeData, bool navigateToNext)
    {
        if (!this.TryGetActiveNavigationItem(routeData, out var activeItem, out var _)) return;
        var allItems = this.Root.EnumAll().Where(item => item.Type is NavigationItemType.Docs or NavigationItemType.Story or NavigationItemType.CustomPage).ToList();
        var ativeIndex = allItems.FindIndex(item => item == activeItem);
        var nextIndex = ativeIndex + (navigateToNext ? +1 : -1);
        if (nextIndex < 0 || allItems.Count <= nextIndex) return;
        var nextItem = allItems[nextIndex];
        this.NavigateTo(allItems[nextIndex]);
    }

    /// <summary>
    /// Marks that the last visited navigation path did not match any known item.
    /// </summary>
    internal void NotifyLastVisitedWasUnknown()
    {
        this._LastNavigatedWasunknown = true;
    }

    /// <summary>
    /// Navigates back to the last successfully visited item, or to the default story if none is recorded.
    /// </summary>
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

    /// <summary>
    /// Returns the list of recently visited navigation items from history.
    /// </summary>
    internal ValueTask<IEnumerable<NavigationListItem>> GetHistoryItemsAsync()
    {
        return this._NavigationHistory.GetItemsAsync();
    }

    /// <summary>
    /// Records the specified navigation item as the most recently visited and adds it to history.
    /// </summary>
    /// <param name="active">The navigation item that was just visited.</param>
    internal async ValueTask AddHistoryAsync(NavigationTreeItem active)
    {
        this._LastNavigated = active;
        await this._NavigationHistory.AddAsync(this.Root, active);
    }

    /// <summary>
    /// Clears all navigation history.
    /// </summary>
    internal async ValueTask ClearHistoryAsync()
    {
        await this._NavigationHistory.ClearAsync();
    }

    /// <summary>
    /// Searches all navigatable items for entries whose caption contains any of the specified keywords.
    /// </summary>
    /// <param name="keywords">The keywords to search for, or <c>null</c> to return no results.</param>
    internal IEnumerable<NavigationListItem> Search(IEnumerable<string>? keywords)
    {
        if (keywords == null || keywords.Where(word => !string.IsNullOrEmpty(word)).Any() == false) return Enumerable.Empty<NavigationListItem>();
        var results = new List<NavigationListItem>();
        this.SearchCore(this.Root, keywords, results);
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
