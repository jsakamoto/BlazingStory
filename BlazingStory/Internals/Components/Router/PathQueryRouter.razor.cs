using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazingStory.Internals.Components.Router;

public partial class PathQueryRouter : ComponentBase
{
    #region Public Properties

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private NavigationManager? NavigationManager { get; set; }

    #endregion Private Properties

    #region Private Fields

    private QueryRouteData _RouteData = new(view: "", parameter: "");

    #endregion Private Fields

    #region Protected Methods

    protected override void OnInitialized()
    {
        if (this.NavigationManager is not null)
        {
            this.NavigationManager.LocationChanged += this.NavigationManager_LocationChanged;

            this.NavigationManager_LocationChanged(this, new LocationChangedEventArgs(this.NavigationManager.Uri, false));
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs args)
    {
        var newRouteData = new QueryRouteData(new Uri(args.Location), queryName: "path");

        if (this._RouteData != newRouteData)
        {
            this._RouteData = newRouteData;
            this.StateHasChanged();
        }
    }

    #endregion Private Methods
}
