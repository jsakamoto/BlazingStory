using BlazingStory.Types;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Models;

internal class ProbingStoryContext
{
    public List<Story> StoryDataSet { get; } = new();

    public void RegisterStory(string name, RenderFragment<StoryContext> renderFragment)
    {
        this.StoryDataSet.Add(new Story(name, new(), renderFragment));
    }
}