using BlazingStory.McpServer.ResultTypes;
using BlazingStory.McpServer.Test._Fixtures;

namespace BlazingStory.McpServer.Test;

internal class StoriesToolTest
{
    [Test]
    public async Task GetComponents_ReturnsTheCatalogedComponentWithItsXmlDocSummary_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        await using var client = await host.CreateMcpClientAsync();

        var result = await client.CallToolAsync("getComponents");
        var components = McpServerTestHost.GetStructuredContent<ComponentSummariesResult>(result).Components.ToArray();

        var alertBox = components.Single(c => c.ComponentName == "AlertBox");
        alertBox.ComponentType.Is("BlazingStoryMcpApp1.Components.AlertBox");
        alertBox.Summary.Is("Displays a short, prominent message to the user.");
    }

    [Test]
    public async Task GetComponentParameters_ReturnsParameterMetadataIncludingOptionsAndRequiredness_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        await using var client = await host.CreateMcpClientAsync();

        var result = await client.CallToolAsync("getComponentParameters", new Dictionary<string, object?> { ["componentName"] = "AlertBox" });
        var parameters = McpServerTestHost.GetStructuredContent<ComponentParameterResult>(result).Parameters.ToArray();

        var title = parameters.Single(p => p.Name == "Title");
        title.Required.IsTrue();
        title.Summary.Is("The headline text rendered at the top of the alert.");

        // An enum-typed parameter reports its selectable values so an agent can pick a valid one.
        // The values arrive quoted, the same way they are shown in the "Controls" addon panel.
        var severity = parameters.Single(p => p.Name == "Severity");
        severity.Required.IsFalse();
        severity.ParameterOptions.Is("\"Info\"", "\"Warning\"", "\"Error\"");
    }

    [Test]
    public async Task GetComponentStories_ReturnsEveryStoryWithItsDescriptionAndCodeSnippet_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        await using var client = await host.CreateMcpClientAsync();

        var result = await client.CallToolAsync("getComponentStories", new Dictionary<string, object?> { ["componentName"] = "AlertBox" });
        var stories = McpServerTestHost.GetStructuredContent<ComponentStoriesResult>(result).Stories.ToArray();

        stories.Select(s => s.Name).Is("Info", "Warning");
        stories.All(s => s.Title == "Components/AlertBox").IsTrue();

        var warning = stories.Single(s => s.Name == "Warning");
        warning.Description.Contains("needs the user's attention").IsTrue();
        // The code snippet is the story's markup with the story's arguments applied.
        warning.CodeSnippet.Contains("<AlertBox").IsTrue();
        warning.CodeSnippet.Contains("Warning").IsTrue();
    }

    [Test]
    public async Task GetComponentParameters_ForAnUnknownComponent_ReportsAToolError_Test()
    {
        await using var host = await McpServerTestHost.StartAsync();
        await using var client = await host.CreateMcpClientAsync();

        var result = await client.CallToolAsync("getComponentParameters", new Dictionary<string, object?> { ["componentName"] = "NoSuchComponent" });

        (result.IsError ?? false).IsTrue();
        result.Content.OfType<ModelContextProtocol.Protocol.TextContentBlock>()
            .Any(block => block.Text.Contains("NoSuchComponent")).IsTrue();
    }
}
