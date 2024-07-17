using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.Router;

public partial class QueryRouteView : ComponentBase
{
    #region Public Properties

    [CascadingParameter]
    public QueryRouteData? RouteData { get; set; }

    [Parameter, EditorRequired]
    public IEnumerable<QueryRoute>? Routes { get; set; }

    #endregion Public Properties
}
