using BlazingStory.Internals.Services.Navigation;
using BlazingStory.Test._Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Test.Internals.Services.Navigation;

internal class NavigationServiceTest
{
    [Test]
    public async Task Search_with_Empty_Test()
    {
        // Given
        await using var host = new TestHost();
        var navService = host.Services.GetRequiredService<NavigationService>();
        navService.BuildNavigationTree(TestHelper.ExampleStories1, null);

        // When / Then
        navService.Search(null).Any().IsFalse();
        navService.Search(new[] { "" }).Any().IsFalse();
    }

    [Test]
    public async Task Search_Hit_by_Caption_with_Single_Word_Test()
    {
        // Given
        await using var host = new TestHost();
        var navService = host.Services.GetRequiredService<NavigationService>();
        navService.BuildNavigationTree(TestHelper.ExampleStories1, null);

        // When
        var searchResults = navService.Search(new[] { "Def" });

        // Then
        searchResults.Dump().Is("Story | Default Button | Examples/Button");
    }

    [Test]
    public async Task Search_Hit_by_Caption_with_MultiWord_Test()
    {
        // Given
        await using var host = new TestHost();
        var navService = host.Services.GetRequiredService<NavigationService>();
        navService.BuildNavigationTree(TestHelper.ExampleStories1, null);

        // When
        var searchResults = navService.Search(new[] { "lec", "def" }); // NOTE: "def" should match "Default" (ignore case).

        // Then
        searchResults.Dump().Is(
            "Story | Default Button | Examples/Button",
            "Component | Select | Examples");
    }

    [Test]
    public async Task Search_Hit_by_Caption_of_Component_Test()
    {
        // Given
        await using var host = new TestHost();
        var navService = host.Services.GetRequiredService<NavigationService>();
        navService.BuildNavigationTree(TestHelper.ExampleStories1, null);

        // When
        var searchResults = navService.Search(new[] { "button" });

        // Then
        searchResults.Dump().Is("Component | Button | Examples");
    }
}
