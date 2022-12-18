using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Pages;

public partial class Index
{
    [CascadingParameter]
    public StoriesStore? StoriesStore { get; set; }

    [CascadingParameter]
    public PathQueryRoutData? RouteData { get; set; }

    protected override void OnInitialized()
    {
    }
}
