using System.Web;
using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazingStory.Internals.Components.Router;

public partial class IdQueryRouter : ComponentBase, IDisposable
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

    #region Public Methods

    public void Dispose()
    {
        if (this.NavigationManager is not null)
        {
            this.NavigationManager.LocationChanged -= this.NavigationManager_LocationChanged;
        }
    }

    #endregion Public Methods

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
        var queryStrings = HttpUtility.ParseQueryString(new Uri(args.Location).Query);
        var viewMode = queryStrings["viewMode"] ?? "";
        var id = queryStrings["id"] ?? "";

        var newRouteData = new QueryRouteData(view: viewMode, parameter: id);

        if (this._RouteData != newRouteData)
        {
            this._RouteData = newRouteData;
            this.StateHasChanged();
        }
    }

    #endregion Private Methods
}
