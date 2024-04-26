using BlazingStory.Components;
using BlazingStory.Test._Fixtures;
using BlazingStory.Test._Fixtures.Components;
using Bunit;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Test.Components;

internal class BlazingStoryServerComponentTest
{
    [TestCase("http://example.com/", "", "IndexPage")]
    [TestCase("http://example.com/", "index.html?foo=bar", "IndexPage")]
    [TestCase("http://example.com/", "iframe.html?foo=bar", "IFramePage")]
    [TestCase("http://example.com/app1/", "", "IndexPage")]
    [TestCase("http://example.com/app1/", "index.html?foo=bar", "IndexPage")]
    [TestCase("http://example.com/app1/", "iframe.html?foo=bar", "IFramePage")]
    public async Task Render_DependOnAppSubPath_Test(string baseUri, string path, string componentName)
    {
        await using var host = new TestHost();
        var navigationManager = host.Services.GetRequiredService<TestNavigationManager>();
        navigationManager.SetUrls(baseUri, uri: baseUri + path);

        var cut = host.BunitContext.RenderComponent<BlazingStoryServerComponent<IndexPage, IFramePage>>();

        cut.MarkupMatches($"<div>This is an {componentName}</div>");
    }
}
