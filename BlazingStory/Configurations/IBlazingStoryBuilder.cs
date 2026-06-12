using BlazingStory.Addons;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Configurations;

public interface IBlazingStoryBuilder
{
    IServiceCollection Services { get; }

    IAddonStore Addons { get; }

    BlazingStoryOptions Options { get; }
}
