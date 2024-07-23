using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Pages;

public partial class FallBackPage : ComponentBase
{
    #region Protected Properties

    [CascadingParameter]
    protected IServiceProvider Services { get; init; } = default!;

    [CascadingParameter]
    protected QueryRouteData RouteData { get; init; } = default!;

    #endregion Protected Properties

    #region Protected Methods

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            var navService = this.Services?.GetRequiredService<NavigationService>();
            navService?.NavigateToDefaultStory(this.RouteData);
        }
    }

    #endregion Protected Methods
}
