using BlazingStory.Internals.JavaScriptAPI;
using BlazingStory.Internals.Services.Navigation;
using BlazingStory.Test._Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Test.Internals.JavaScriptAPI;

internal class BlazingStoryAPITest
{
    [Test]
    public async Task GetStoryIndex_Returns_Index_From_StoriesStore_Test()
    {
        // Given
        await using var host = new TestHost();
        var navService = host.Services.GetRequiredService<NavigationService>();
        navService.BuildNavigationTree(
            TestHelper.GetExampleStories1(host.Services),
            TestHelper.GetExampleCustomPages1(host.Services),
            [], null);
        var blazingStoryApi = new BlazingStoryAPI(host.Services);

        // When
        var storyIndex = blazingStoryApi.GetStoryIndex();

        // Then
        storyIndex.V.Is(1);

        var entries = storyIndex.Entries
            .OrderBy(entry => entry.Key)
            .Select(entry => (entry.Key, entry.Value.Id, entry.Value.Title, entry.Value.Name, entry.Value.Type))
            .ToArray();

        entries.Is(
            ("examples-button--default-button", "examples-button--default-button", "Examples/Button", "Default Button", "story"),
            ("examples-button--docs", "examples-button--docs", "Examples/Button", "Docs", "docs"),
            ("examples-button--primary-button", "examples-button--primary-button", "Examples/Button", "Primary Button", "story"),
            ("examples-getting-started", "examples-getting-started", "Examples/Getting Started", "Getting Started", "docs"),
            ("examples-select--docs", "examples-select--docs", "Examples/Select", "Docs", "docs"),
            ("examples-select--select", "examples-select--select", "Examples/Select", "Select", "story"),
            ("welcome", "welcome", "Welcome", "Welcome", "docs")
            );
    }
}
