using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Pages;

public partial class FallBackPage : ComponentBase
{
    [CascadingParameter]
    protected IServiceProvider Services { get; init; } = default!;

    [CascadingParameter]
    protected QueryRouteData RouteData { get; init; } = default!;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            var navService = this.Services?.GetRequiredService<NavigationService>();
            navService?.NavigateToDefaultStory(this.RouteData);
        }
    }
}
