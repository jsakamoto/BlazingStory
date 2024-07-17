using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Pages.Canvas;

public partial class CanvasFrameNothingToAddress : ComponentBase
{
    #region Public Properties

    [CascadingParameter]
    public QueryRouteData? RouteData { get; set; }

    #endregion Public Properties
}
