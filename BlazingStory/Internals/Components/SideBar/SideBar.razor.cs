using BlazingStory.Components;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Components.SideBar;

public partial class SideBar : ComponentBase
{
    #region Protected Properties

    [CascadingParameter]
    protected StoriesStore StoriesStore { get; init; } = default!;

    [CascadingParameter]
    protected QueryRouteData RouteData { get; init; } = default!;

    [CascadingParameter]
    protected IServiceProvider Services { get; init; } = default!;

    [CascadingParameter]
    protected BlazingStoryApp BlazingStoryApp { get; init; } = default!;

    #endregion Protected Properties

    #region Private Fields

    private bool _SearchMode = false;

    private NavigationTreeItem _NavigationRoot = new();

    #endregion Private Fields

    #region Protected Methods

    protected override void OnInitialized()
    {
        var expandedNavigationPath = this.RouteData.RouteToStoryOrDocs ? this.RouteData.Parameter : null;
        var navigationService = this.Services.GetRequiredService<NavigationService>();
        this._NavigationRoot = navigationService.BuildNavigationTree(this.StoriesStore.StoryContainers, expandedNavigationPath);
    }

    #endregion Protected Methods
}
