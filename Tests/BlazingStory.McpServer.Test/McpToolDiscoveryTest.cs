using BlazingStory.McpServer.Test._Fixtures;

namespace BlazingStory.McpServer.Test;

internal class McpToolDiscoveryTest
{
    [Test]
    public async Task ListTools_ExposesEveryBlazingStoryTool_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        await using var client = await host.CreateMcpClientAsync();

        var tools = await client.ListToolsAsync();

        tools.Select(t => t.Name).OrderBy(name => name, StringComparer.Ordinal).Is(
            "getComponentParameters",
            "getComponentStories",
            "getComponents",
            "getCustomPageContent",
            "getCustomPages",
            "searchCustomPages");
    }

    [Test]
    public async Task ListTools_EveryToolIsDescribedAndHasAnOutputSchema_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        await using var client = await host.CreateMcpClientAsync();

        var tools = await client.ListToolsAsync();

        // The tools opt into "UseStructuredContent", so every one of them has to advertise an
        // output schema for clients to validate the structured content against.
        foreach (var tool in tools)
        {
            Assert.That(tool.Description, Is.Not.Null.And.Not.Empty, $"The '{tool.Name}' tool has no description.");
            Assert.That(tool.ProtocolTool.OutputSchema, Is.Not.Null, $"The '{tool.Name}' tool has no output schema.");
        }
    }

    [Test]
    public async Task ListTools_ToolsThatTakeArgumentsDeclareThemAsRequired_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        await using var client = await host.CreateMcpClientAsync();

        var tools = (await client.ListToolsAsync()).ToDictionary(t => t.Name);

        tools["getComponentParameters"].ProtocolTool.InputSchema.ToString().Contains("componentName").IsTrue();
        tools["getComponentStories"].ProtocolTool.InputSchema.ToString().Contains("componentName").IsTrue();
        tools["getCustomPageContent"].ProtocolTool.InputSchema.ToString().Contains("pageTitle").IsTrue();
        tools["searchCustomPages"].ProtocolTool.InputSchema.ToString().Contains("query").IsTrue();
    }
}
