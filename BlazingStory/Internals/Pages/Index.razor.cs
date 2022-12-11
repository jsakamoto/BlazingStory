using BlazingStory.Internals.Services;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Pages;

public partial class Index
{
    [CascadingParameter]
    public StoriesStore? StoriesStore { get; set; }

    protected override void OnInitialized()
    {
        Console.WriteLine($"StoriesStore: {this.StoriesStore?.StoryContainers.Count() ?? -1}");
    }
}
