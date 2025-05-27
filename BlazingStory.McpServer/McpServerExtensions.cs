using System.Diagnostics.CodeAnalysis;
using BlazingStory.Internals.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.McpServer;

/// <summary>
/// Provides extension methods for configuring MCP servers of Blazing Story with dependency injection.
/// </summary>
public static class McpServerExtensions
{
    /// <summary>
    /// Add the Model Context Protocol (MCP) server of Blazing Story to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the server to.</param>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    public static IServiceCollection AddBlazingStoryMcpServer(this IServiceCollection services)
    {
        services
            .AddMcpServer(options => { })
            .WithHttpTransport(options => { })
            .WithTools<StoriesTool>()
            .WithTools<TimeTool>();

        services.AddScoped((_) => new StoriesStore());

        return services;
    }

    /// <summary>
    /// Sets up endpoints for handling MCP protocol over HTTP in Blazing Story apps.
    /// </summary>
    /// <param name="endpoints">The web application to attach MCP HTTP endpoints.</param>
    /// <param name="pattern">The route pattern prefix to map to. By default, it is "mcp/blazingstory".</param>
    /// <returns>Returns a builder for configuring additional endpoint conventions like authorization policies.</returns>
    public static IEndpointConventionBuilder MapBlazingStoryMcp(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern = "mcp/blazingstory")
    {
        return endpoints.MapMcp(pattern);
    }
}
