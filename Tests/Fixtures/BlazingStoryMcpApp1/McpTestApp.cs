using BlazingStory.Components;
using BlazingStory.McpServer;
using BlazingStoryMcpApp1.Components.Pages;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.AspNetCore;

namespace BlazingStoryMcpApp1;

/// <summary>
/// Builds a Blazing Story server app with the MCP server enabled, wired up the same way the
/// "BlazingStoryServer" project template and the MCPServer sample do.
/// </summary>
public static class McpTestApp
{
    /// <summary>
    /// Creates the app, listening on an ephemeral loopback port.
    /// </summary>
    /// <param name="configureHttpServerTransportOptions">
    /// An optional action to configure the MCP HTTP transport, allowing tests to exercise
    /// non-default session modes.
    /// </param>
    public static WebApplication Create(Action<HttpServerTransportOptions>? configureHttpServerTransportOptions = null)
    {
        var builder = WebApplication.CreateSlimBuilder();
        builder.Logging.ClearProviders();
        builder.WebHost.UseUrls("http://127.0.0.1:0");

        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
        builder.Services.AddBlazingStoryMcpServer(
            configureHttpServerTransportOptions: configureHttpServerTransportOptions);

        var app = builder.Build();

        app.MapBlazingStoryMcp();
        app.UseRouting();
        app.UseAntiforgery();
        app.MapRazorComponents<BlazingStoryServerComponent<IndexPage, IFramePage>>()
            .AddInteractiveServerRenderMode();

        return app;
    }
}
