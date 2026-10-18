using System.Diagnostics.CodeAnalysis;
using BlazingStory.Internals.Services;
using BlazingStory.McpServer.Internals;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Server;

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
    /// <param name="configureMcpServerOptions">An optional action to configure the <see cref="McpServerOptions"/> for the MCP server.</param>
    /// <param name="configureHttpServerTransportOptions">
    /// An optional action to configure the <see cref="HttpServerTransportOptions"/> for the MCP server.
    /// Blazing Story's tools are stateless request/response lookups, so the transport defaults to
    /// <see cref="HttpServerSessionMode.Stateless"/>. Use this to opt into
    /// <see cref="HttpServerSessionMode.StatefulForInitializeClients"/> if you need to serve MCP clients that
    /// still require a session from the legacy "initialize" handshake.
    /// </param>
    /// <returns>An <see cref="IMcpServerBuilder"/> that can be used to further configure the MCP server.</returns>
    public static IMcpServerBuilder AddBlazingStoryMcpServer(
        this IServiceCollection services,
        Action<McpServerOptions>? configureMcpServerOptions = null,
        Action<HttpServerTransportOptions>? configureHttpServerTransportOptions = null)
    {
        services.AddScoped((_) => new StoriesStore());
        services.AddScoped((_) => new CustomPageStore());
        services.AddSingleton<CustomPageContentCache>();
        return services
            .AddMcpServer(options => configureMcpServerOptions?.Invoke(options))
            .WithHttpTransport(options =>
            {
                options.SessionMode = HttpServerSessionMode.Stateless;
                configureHttpServerTransportOptions?.Invoke(options);
            })
            .WithTools<StoriesTool>()
            .WithTools<CustomPagesTool>();
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
