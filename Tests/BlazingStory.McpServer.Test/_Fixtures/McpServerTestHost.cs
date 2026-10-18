using System.Text.Json;
using BlazingStoryMcpApp1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace BlazingStory.McpServer.Test._Fixtures;

/// <summary>
/// Hosts the <see cref="BlazingStoryMcpApp1"/> fixture on a real Kestrel loopback endpoint and
/// connects to it with the official MCP client, so the tests exercise the whole HTTP transport
/// rather than invoking the tool methods directly.
/// </summary>
internal sealed class McpServerTestHost : IAsyncDisposable
{
    private readonly WebApplication _App;

    private McpServerTestHost(WebApplication app, Uri baseAddress)
    {
        this._App = app;
        this.BaseAddress = baseAddress;
    }

    internal Uri BaseAddress { get; }

    internal Uri McpEndpoint => new(this.BaseAddress, "/mcp/blazingstory");

    internal static async ValueTask<McpServerTestHost> StartAsync(Action<HttpServerTransportOptions>? configureHttpServerTransportOptions = null)
    {
        var app = McpTestApp.Create(configureHttpServerTransportOptions);
        await app.StartAsync();

        var addresses = app.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>();
        var address = addresses?.Addresses.FirstOrDefault() ?? throw new InvalidOperationException("The test host did not report a bound address.");
        return new McpServerTestHost(app, new Uri(address));
    }

    internal async ValueTask<McpClient> CreateMcpClientAsync(CancellationToken cancellationToken = default)
    {
        var transport = new HttpClientTransport(new HttpClientTransportOptions { Endpoint = this.McpEndpoint });
        return await McpClient.CreateAsync(transport, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Calls a tool and deserializes its structured content, which is what MCP clients that
    /// understand output schemas actually consume.
    /// </summary>
    internal static T GetStructuredContent<T>(CallToolResult result)
    {
        (result.IsError ?? false).IsFalse();
        var structuredContent = result.StructuredContent ?? throw new InvalidOperationException("The tool result had no structured content.");
        return structuredContent.Deserialize<T>(new JsonSerializerOptions(JsonSerializerDefaults.Web))
            ?? throw new InvalidOperationException("The structured content could not be deserialized.");
    }

    public async ValueTask DisposeAsync()
    {
        await this._App.StopAsync();
        await this._App.DisposeAsync();
    }
}
