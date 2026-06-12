using System.ComponentModel;
using BlazingStory.Components;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using BlazingStory.McpServer.Internals;
using BlazingStory.McpServer.ResultTypes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace BlazingStory.McpServer;

[McpServerToolType]
internal class CustomPagesTool
{
    private readonly IServiceProvider _services;

    private readonly CustomPageContentCache _cache;

    public CustomPagesTool(IServiceProvider services, CustomPageContentCache cache)
    {
        this._services = services;
        this._cache = cache;
    }

    [McpServerTool(Name = "getCustomPages", UseStructuredContent = true)]
    [Description("""
        Retrieves a list of all custom pages and markdown pages available in this UI catalog.
        Custom pages typically contain general information such as getting started guides, setup instructions, or documentation that is not tied to a specific component.
        For each page, it returns the title.
        To retrieve the full content of a specific page, use the `getCustomPageContent` tool, or use `searchCustomPages` to search across all pages by keyword.
        """)]
    public async Task<CustomPageSummariesResult> GetCustomPagesAsync()
    {
        using var scope = this._services.CreateScope();
        var customPageStore = await this.BuildCustomPageStoreAsync(scope);

        var pages = customPageStore.CustomPageContainers.Select(c => new CustomPageSummary(
            Title: c.Title
        )).ToList();

        return new(pages);
    }

    [McpServerTool(Name = "getCustomPageContent", UseStructuredContent = true)]
    [Description("""
        Retrieves the full rendered HTML content of a specific custom page or markdown page, identified by its title.
        The result includes the page title and the complete rendered HTML content.
        Use this tool after calling `getCustomPages` to get the list of available pages and their titles.
        """)]
    public async Task<CustomPageDetail> GetCustomPageContentAsync(
        [Description("The title of the custom page to retrieve content for (e.g., \"Getting Started\").")]
        string pageTitle)
    {
        using var scope = this._services.CreateScope();
        var customPageStore = await this.BuildCustomPageStoreAsync(scope);

        var container = customPageStore.CustomPageContainers.FirstOrDefault(c =>
            c.Title.Equals(pageTitle, StringComparison.OrdinalIgnoreCase));
        if (container is null) throw new McpException($"Custom page '{pageTitle}' not found.");

        return await this.GetOrRenderPageAsync(container, scope);
    }

    [McpServerTool(Name = "searchCustomPages", UseStructuredContent = true)]
    [Description("""
        Searches across all custom pages and markdown pages for content matching the given query.
        Returns matching pages ordered by relevance, each with a contextual text snippet around the matched terms.
        Use this tool to find relevant documentation, setup instructions, or guides without needing to know the exact page title.
        To retrieve the full content of a matching page, use the `getCustomPageContent` tool with the page title from the results.
        """)]
    public async Task<CustomPageSearchResult> SearchCustomPagesAsync(
        [Description("The search query to match against custom page content (e.g., \"getting started\", \"setup instructions\").")]
        string query)
    {
        using var scope = this._services.CreateScope();
        var customPageStore = await this.BuildCustomPageStoreAsync(scope);

        await this.EnsureCachePopulatedAsync(customPageStore, scope);

        var entries = this._cache.GetAllEntries().ToArray();
        var entriesByPath = entries.ToDictionary(e => e.NavigationPath);
        var index = this._cache.GetOrBuildIndex();
        var scored = Bm25Scorer.Search(index, query);

        var results = scored
            .Select(hit => entriesByPath.TryGetValue(hit.Key, out var entry) ? entry : null)
            .Where(entry => entry is not null)
            .Select(entry =>
            {
                var plainText = Bm25Scorer.StripHtmlTags(entry!.HtmlContent);
                var snippet = Bm25Scorer.ExtractSnippet(plainText, query);
                return new CustomPageSearchHit(entry.Title, snippet);
            }).ToList();

        return new(results);
    }

    private async Task<CustomPageDetail> GetOrRenderPageAsync(CustomPageContainer container, IServiceScope scope)
    {
        if (this._cache.TryGetEntry(container.NavigationPath, out var cached))
        {
            return new CustomPageDetail(cached.Title, cached.HtmlContent);
        }

        var html = await CustomStaticHtmlRenderer.RenderToHtmlStringAsync(
            container.CustomPageRazorDescriptor.TypeOfCustomPageRazor,
            scope.ServiceProvider,
            ParameterView.Empty);

        html = string.Join('\n', html.Split('\n').Select(line => line.Trim(' ', '\t', '\n')));

        var entry = new CustomPageCacheEntry(container.Title, container.NavigationPath, html);
        this._cache.SetEntry(container.NavigationPath, entry);

        return new CustomPageDetail(entry.Title, entry.HtmlContent);
    }

    private async Task EnsureCachePopulatedAsync(CustomPageStore customPageStore, IServiceScope scope)
    {
        foreach (var container in customPageStore.CustomPageContainers)
        {
            if (!this._cache.TryGetEntry(container.NavigationPath, out _))
            {
                await this.GetOrRenderPageAsync(container, scope);
            }
        }
    }

    private async ValueTask<CustomPageStore> BuildCustomPageStoreAsync(IServiceScope scope)
    {
        var endpointDataSource = this._services.GetRequiredService<EndpointDataSource>();

        var indexComponentType = endpointDataSource.Endpoints
            .SelectMany(endpoint => endpoint.Metadata)
            .OfType<RootComponentMetadata>()
            .Where(metaData => metaData.Type.IsGenericType && metaData.Type.GetGenericTypeDefinition() == typeof(BlazingStoryServerComponent<,>))
            .Select(metaData => metaData.Type.GenericTypeArguments.First())
            .FirstOrDefault();

        var scopedServices = scope.ServiceProvider;

        if (indexComponentType is not null)
        {
            await CustomStaticHtmlRenderer.RenderToHtmlStringAsync(indexComponentType, scopedServices, ParameterView.Empty);
        }

        return scopedServices.GetRequiredService<CustomPageStore>();
    }
}
