using BlazingStory.Addons;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Configurations;

internal class BlazingStoryBuilder : IBlazingStoryBuilder
{
    public IServiceCollection Services { get; } = new ServiceCollection();

    public IAddonStore Addons { get; } = new AddonStore();

    public BlazingStoryOptions Options { get; }

    public BlazingStoryBuilder(BlazingStoryOptions options)
    {
        this.Options = options;
    }
}
