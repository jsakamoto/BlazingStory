using BlazingStory.Addons;
using BlazingStory.Addons.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Configurations;

internal class BlazingStoryBuilder : IBlazingStoryBuilder
{
    public IServiceCollection Services { get; }

    public IAddonStore Addons { get; }

    public BlazingStoryOptions Options { get; }

    public BlazingStoryBuilder(IServiceCollection services, AddonStore addonStore, BlazingStoryOptions options)
    {
        this.Services = services;
        this.Addons = addonStore;
        this.Options = options;
    }
}
