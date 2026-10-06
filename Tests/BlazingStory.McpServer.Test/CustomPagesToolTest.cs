using BlazingStory.McpServer.ResultTypes;
using BlazingStory.McpServer.Test._Fixtures;

namespace BlazingStory.McpServer.Test;

internal class CustomPagesToolTest
{
    [Test]
    public async Task GetCustomPages_ReturnsThePagesDeclaredWithTheCustomPageAttribute_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        await using var client = await host.CreateMcpClientAsync();

        var result = await client.CallToolAsync("getCustomPages");
        var customPages = McpServerTestHost.GetStructuredContent<CustomPageSummariesResult>(result).CustomPages.ToArray();

        customPages.Select(p => p.Title).Contains("Usage Guide").IsTrue();
    }

    [Test]
    public async Task GetCustomPageContent_ReturnsTheRenderedHtmlOfThePage_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        await using var client = await host.CreateMcpClientAsync();

        var result = await client.CallToolAsync("getCustomPageContent", new Dictionary<string, object?> { ["pageTitle"] = "Usage Guide" });
        var detail = McpServerTestHost.GetStructuredContent<CustomPageDetail>(result);

        detail.Title.Is("Usage Guide");
        detail.HtmlContent.Contains("<h2>Usage Guide</h2>").IsTrue();
        detail.HtmlContent.Contains("do not trap focus").IsTrue();
    }

    [Test]
    public async Task GetCustomPageContent_MatchesTheTitleCaseInsensitively_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        await using var client = await host.CreateMcpClientAsync();

        var result = await client.CallToolAsync("getCustomPageContent", new Dictionary<string, object?> { ["pageTitle"] = "usage guide" });

        McpServerTestHost.GetStructuredContent<CustomPageDetail>(result).Title.Is("Usage Guide");
    }

    [Test]
    public async Task SearchCustomPages_FindsAPageByItsBodyText_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        await using var client = await host.CreateMcpClientAsync();

        var result = await client.CallToolAsync("searchCustomPages", new Dictionary<string, object?> { ["query"] = "severity" });
        var hits = McpServerTestHost.GetStructuredContent<CustomPageSearchResult>(result).Results.ToArray();

        var hit = hits.Single(h => h.Title == "Usage Guide");
        Assert.That(hit.Snippet, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task GetCustomPageContent_ForAnUnknownPage_ReportsAToolError_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        await using var client = await host.CreateMcpClientAsync();

        var result = await client.CallToolAsync("getCustomPageContent", new Dictionary<string, object?> { ["pageTitle"] = "No Such Page" });

        (result.IsError ?? false).IsTrue();
        result.Content.OfType<ModelContextProtocol.Protocol.TextContentBlock>()
            .Any(block => block.Text.Contains("No Such Page")).IsTrue();
    }
}
