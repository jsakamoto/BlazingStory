using System.ComponentModel;
using BlazingStory.Components;
using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Docs;
using BlazingStory.McpServer.Internals;
using BlazingStory.McpServer.ResultTypes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;

namespace BlazingStory.McpServer;

[McpServerToolType]
internal class StoriesTool
{
    private readonly IServiceProvider _services;

    public StoriesTool(IServiceProvider services)
    {
        this._services = services;
    }

    [McpServerTool(Name = "getComponents")]
    [Description("""
        Retrieves metadata for all Razor components available in this UI catalog for Blazor applications.
        For each component, it returns basic information such as the component’s name, type (e.g., layout, form, display), and a brief summary describing its purpose or function.
        This tool provides a high-level overview of the components available in the catalog and is typically used to populate component lists, enable search or filter functionality, or generate documentation indexes.
        To explore a specific component in more depth, use the `getComponentParameters` tool to retrieve its configurable parameters, or the `getComponentStories` tool to see practical usage examples with code snippets.
        """)]
    public async Task<IEnumerable<ComponentSummary>> GetComponentsAsync()
    {
        using var scope = this._services.CreateScope();
        var storiesStore = await this.BuildStoriesStoreAsync(scope);
        await Parallel.ForEachAsync(storiesStore.StoryContainers, async (container, token) =>
        {
            await container.UpdateSummaryFromXmlDocCommentAsync();
        });

        var components = storiesStore.StoryContainers.Select(c => new ComponentSummary
        (
            ComponentName: c.TargetComponentType.Name,
            ComponentType: c.TargetComponentType.FullName ?? "(unknown)",
            Summary: c.Summary.Value
        ));

        return components;
    }

    [McpServerTool(Name = "getComponentParameters")]
    [Description("""
        Retrieves all parameters of a specific Razor component identified by its component name.
        This includes detailed information for each parameter such as the parameter name, type, summary/description, and any available option values (e.g., for enum or select-type parameters).
        Use this tool to understand how to configure a given component programmatically or within markup by exploring its full parameter schema.
        This is especially useful for dynamically generating documentation, building design systems, or enabling component-level code generation.
        """)]
    public async Task<ComponentParameterResult> GetComponentParametersAsync(
        [Description("The name of the component to retrieve parameters for.")]
        string componentName)
    {
        using var scope = this._services.CreateScope();
        var storiesStore = await this.BuildStoriesStoreAsync(scope);
        var container = storiesStore.StoryContainers.FirstOrDefault(c => c.TargetComponentType.Name == componentName);
        if (container is null) return new(Success: false, ErrorMessage: $"Component '{componentName}' not found or parameter info not available.", Parameters: []);

        var story = container.Stories.FirstOrDefault();
        if (story is null) return new(Success: false, ErrorMessage: $"Parameter info of the component '{componentName}' not available since it has no stories.", Parameters: []);

        await Parallel.ForEachAsync(story.Context.Parameters, async (parameter, token) =>
        {
            await parameter.UpdateSummaryFromXmlDocCommentAsync();
        });

        var parameters = story.Context.Parameters.Select(p =>
        {
            var typeStrings = p.GetParameterTypeStrings();
            return new ComponentParameter
            (
                p.Name,
                Type: typeStrings.FirstOrDefault() ?? "(unknown)",
                p.Required,
                Summary: p.Summary.Value,
                ParameterOptions: typeStrings.Skip(1)
            );
        });

        return new(parameters);
    }

    [McpServerTool(Name = "getComponentStories")]
    [Description("""
        Retrieves all stories associated with a specific Razor component identified by its component name.
        Each story typically represents a usage example or scenario for the component, and includes metadata such as the story name, title, description (explaining how and when to use the component in that context), and a code snippet demonstrating its usage.
        This tool is useful for programmatically accessing component usage examples, generating documentation, or integrating component previews in design systems and developer tooling.
        It helps to provide concrete, contextual usage guidance beyond the raw parameter definitions, making it easier to understand the component’s behavior in real-world UI scenarios.
        """)]
    public async Task<ComponentStoriesResult> GetComponentStoriesAsync(
        [Description("The name of the component to retrieve stories for.")]
        string componentName)
    {
        using var scope = this._services.CreateScope();
        var storiesStore = await this.BuildStoriesStoreAsync(scope);
        var container = storiesStore.StoryContainers.FirstOrDefault(c => c.TargetComponentType.Name == componentName);
        if (container is null) return new(Success: false, ErrorMessage: $"Component '{componentName}' not found or has no stories.", Stories: []);

        var projectionTasks = container.Stories.Select(async (BlazingStory.Internals.Models.Story s) =>
        {
            var originalCodeText = await StoriesRazorSource.GetSourceCodeAsync(s);
            var transformedCodeText = StoriesRazorSource.UpdateSourceTextWithArgument(s, originalCodeText);

            var description = await CustomStaticHtmlRenderer.RenderToHtmlStringAsync(s.Description, scope.ServiceProvider);
            description = string.Join('\n', description.Split('\n').Select(line => line.Trim(' ', '\t', '\n')));

            return new ComponentStory
            (
                s.Name,
                s.Title,
                Description: description,
                CodeSnippet: transformedCodeText
            );
        }).ToArray();

        await Task.WhenAll(projectionTasks);
        var stories = projectionTasks.Select(t => t.Result);

        return new(stories);
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
            await CustomStaticHtmlRenderer.RenderToHtmlStringAsync(indexComponentType, scopedServices, ParameterView.Empty);
        }

        return scopedServices.GetRequiredService<StoriesStore>();
    }
}
