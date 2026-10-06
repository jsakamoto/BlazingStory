using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BlazingStory.McpServer.Test._Fixtures;
using ModelContextProtocol.AspNetCore;

namespace BlazingStory.McpServer.Test;

/// <summary>
/// The MCP 2.x HTTP transport is stateless by default. These tests pin that default down and
/// verify that MCP clients still speaking the older "initialize" handshake keep working, since
/// a stateless server issues no session id for them to echo back.
/// </summary>
internal class HttpTransportSessionModeTest
{
    private const string DownLevelProtocolVersion = "2025-06-18";

    [Test]
    public async Task ByDefault_TheServerIsStatelessAndIssuesNoSessionId_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        using var httpClient = new HttpClient();

        using var response = await PostJsonRpcAsync(httpClient, host, """
            {"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2025-06-18","capabilities":{},"clientInfo":{"name":"test","version":"1.0"}}}
            """);

        response.IsSuccessStatusCode.IsTrue();
        response.Headers.Contains("Mcp-Session-Id").IsFalse();
    }

    [Test]
    public async Task ADownLevelClient_CanCallAToolWithoutEverOpeningASession_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        using var httpClient = new HttpClient();

        using var response = await PostJsonRpcAsync(httpClient, host, """
            {"jsonrpc":"2.0","id":2,"method":"tools/call","params":{"name":"getComponents","arguments":{}}}
            """);

        var payload = await ReadJsonRpcPayloadAsync(response);
        payload.RootElement.TryGetProperty("error", out _).IsFalse();
        payload.RootElement
            .GetProperty("result").GetProperty("structuredContent").GetProperty("components")
            .EnumerateArray().Any(c => c.GetProperty("componentName").GetString() == "AlertBox")
            .IsTrue();
    }

    [Test]
    public async Task WhenConfiguredForStatefulSessions_ADownLevelInitializeGetsASessionId_Test()
    {
        await using var host = await McpServerTestHost.StartAsync(
            options => options.SessionMode = HttpServerSessionMode.StatefulForInitializeClients);
        using var httpClient = new HttpClient();

        using var response = await PostJsonRpcAsync(httpClient, host, """
            {"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2025-06-18","capabilities":{},"clientInfo":{"name":"test","version":"1.0"}}}
            """);

        response.IsSuccessStatusCode.IsTrue();
        response.Headers.Contains("Mcp-Session-Id").IsTrue();
    }

    private static async Task<HttpResponseMessage> PostJsonRpcAsync(HttpClient httpClient, McpServerTestHost host, string json)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, host.McpEndpoint)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        };
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        request.Headers.Add("MCP-Protocol-Version", DownLevelProtocolVersion);
        return await httpClient.SendAsync(request);
    }

    /// <summary>
    /// Reads a JSON-RPC response that may arrive either as plain JSON or as a single
    /// server-sent event, which is how the streamable HTTP transport replies.
    /// </summary>
    private static async Task<JsonDocument> ReadJsonRpcPayloadAsync(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();
        var json = body
            .Split('\n')
            .Select(line => line.Trim('\r'))
            .FirstOrDefault(line => line.StartsWith("data:", StringComparison.Ordinal))
            ?.Substring("data:".Length)
            .Trim()
            ?? body;
        return JsonDocument.Parse(json);
    }
}
