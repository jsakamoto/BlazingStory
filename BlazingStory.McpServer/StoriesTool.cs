using System.ComponentModel;
using System.Text.Json;
using BlazingStory.Components;
using BlazingStory.Internals.Services;
using BlazingStory.McpServer.Internals;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace BlazingStory.McpServer;

[McpServerToolType]
internal class StoriesTool
{
    private readonly IServiceProvider _services;

    private readonly ILoggerFactory _loggerFactory;

    public StoriesTool(IServiceProvider services, ILoggerFactory loggerFactory)
    {
        this._services = services;
        this._loggerFactory = loggerFactory;
    }

    [McpServerTool(Name = "getComponents")]
    [Description("Retrieves the names, types, and summaries of all Razor components for Blazor applications available in this UI catalog.")]
    public async Task<string> GetComponentsAsync()
    {
        using var scope = this._services.CreateScope();
        var storiesStore = await this.BuildStoriesStoreAsync(scope);
        await Parallel.ForEachAsync(storiesStore.StoryContainers, async (container, token) =>
        {
            await container.UpdateSummaryFromXmlDocCommentAsync();
        });

        var components = storiesStore.StoryContainers.Select(c => new
        {
            ComponentName = c.TargetComponentType.Name,
            ComponentType = c.TargetComponentType.FullName,
            Summary = c.Summary.Value
        });
        var json = JsonSerializer.Serialize(components, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return json;
    }

    private async ValueTask<StoriesStore> BuildStoriesStoreAsync(IServiceScope scope)
    {
        var endpointDataSource = this._services.GetRequiredService<EndpointDataSource>();

        var indexComponentType = endpointDataSource.Endpoints
            .SelectMany(endpoint => endpoint.Metadata)
            .OfType<RootComponentMetadata>()
            .Where(metaData => metaData.Type.GetGenericTypeDefinition() == typeof(BlazingStoryServerComponent<,>))
            .Select(metaData => metaData.Type.GenericTypeArguments.First())
            .FirstOrDefault();

        var scopedServices = scope.ServiceProvider;

        // Render the index component to retrieve stories
        if (indexComponentType is not null)
        {
            var navigationManager = scopedServices.GetRequiredService<NavigationManager>() as IHostEnvironmentNavigationManager;

            var serverAddressesFeature = this._services
                .GetRequiredService<IServer>()
                .Features
                .GetRequiredFeature<IServerAddressesFeature>();
            var appUrl = serverAddressesFeature.Addresses.First().TrimEnd('/') + "/";

            navigationManager?.Initialize(appUrl, appUrl);

            await using var customRenderer = new CustomStaticHtmlRenderer(scopedServices, this._loggerFactory);
            await customRenderer.Dispatcher.InvokeAsync(async () =>
            {
                try
                {
                    var content = customRenderer.BeginRenderingComponent(indexComponentType, ParameterView.Empty);
                    await content.QuiescenceTask;
                }
                catch (Exception ex)
                {
                    var logger = this._loggerFactory.CreateLogger<StoriesTool>();
                    logger.LogError(ex, "Failed to render index component.");
                    throw;
                }
            });
        }

        return scopedServices.GetRequiredService<StoriesStore>();
    }
}
