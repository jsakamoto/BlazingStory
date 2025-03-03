using BlazingStory.Internals.Services.Navigation;
using BlazingStory.Test._Fixtures;
using Microsoft.AspNetCore.Components;
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
        navService.BuildNavigationTree(TestHelper.GetExampleStories1(host.Services), [], null);

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
        navService.BuildNavigationTree(TestHelper.GetExampleStories1(host.Services), [], null);

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
        navService.BuildNavigationTree(TestHelper.GetExampleStories1(host.Services), [], null);

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
        navService.BuildNavigationTree(TestHelper.GetExampleStories1(host.Services), [], null);

        // When
        var searchResults = navService.Search(new[] { "button" });

        // Then
        searchResults.Dump().Is("Component | Button | Examples");
    }

    [Test]
    public async Task NavigateToNextComponentItem_Test()
    {
        // Given
        await using var host = new TestHost();
        var navMan = host.Services.GetRequiredService<NavigationManager>();
        var navService = host.Services.GetRequiredService<NavigationService>();
        navService.BuildNavigationTree(TestHelper.GetExampleStories1(host.Services), [], null);

        navService.NavigateToNextComponentItem(new("story", "examples-button--default-button"), navigateToNext: true);
        navMan.Uri.Is("http://localhost/?path=/docs/examples-select--docs");

        navService.NavigateToNextComponentItem(new("docs", "examples-select--docs"), navigateToNext: true);
        navMan.Uri.Is("http://localhost/?path=/docs/examples-select--docs");
    }

    [Test]
    public async Task NavigateToPrevComponentItem_Test()
    {
        // Given
        await using var host = new TestHost();
        var navMan = host.Services.GetRequiredService<NavigationManager>();
        var navService = host.Services.GetRequiredService<NavigationService>();
        navService.BuildNavigationTree(TestHelper.GetExampleStories1(host.Services), [], null);

        navService.NavigateToNextComponentItem(new("docs", "examples-select--docs"), navigateToNext: false);
        navMan.Uri.Is("http://localhost/?path=/docs/examples-button--docs");

        navService.NavigateToNextComponentItem(new("story", "examples-button--primary-button"), navigateToNext: false);
        navMan.Uri.Is("http://localhost/?path=/docs/examples-button--docs");
    }

    [Test]
    public async Task NavigateToNextDocsOrStory_Test()
    {
        // Given
        await using var host = new TestHost();
        var navMan = host.Services.GetRequiredService<NavigationManager>();
        var navService = host.Services.GetRequiredService<NavigationService>();
        navService.BuildNavigationTree(TestHelper.GetExampleStories1(host.Services), [], null);

        navService.NavigateToNextDocsOrStory(new("docs", "examples-button--docs"), navigateToNext: true);
        navMan.Uri.Is("http://localhost/?path=/story/examples-button--default-button");

        navService.NavigateToNextDocsOrStory(new("story", "examples-button--default-button"), navigateToNext: true);
        navMan.Uri.Is("http://localhost/?path=/story/examples-button--primary-button");

        navService.NavigateToNextDocsOrStory(new("story", "examples-button--primary-button"), navigateToNext: true);
        navMan.Uri.Is("http://localhost/?path=/docs/examples-select--docs");

        navService.NavigateToNextDocsOrStory(new("docs", "examples-select--docs"), navigateToNext: true);
        navMan.Uri.Is("http://localhost/?path=/story/examples-select--select");

        navService.NavigateToNextDocsOrStory(new("story", "examples-select--select"), navigateToNext: true);
        navMan.Uri.Is("http://localhost/?path=/story/examples-select--select");
    }

    [Test]
    public async Task NavigateToPrevDocsOrStory_Test()
    {
        // Given
        await using var host = new TestHost();
        var navMan = host.Services.GetRequiredService<NavigationManager>();
        var navService = host.Services.GetRequiredService<NavigationService>();
        navService.BuildNavigationTree(TestHelper.GetExampleStories1(host.Services), [], null);

        navService.NavigateToNextDocsOrStory(new("story", "examples-select--select"), navigateToNext: false);
        navMan.Uri.Is("http://localhost/?path=/docs/examples-select--docs");

        navService.NavigateToNextDocsOrStory(new("docs", "examples-select--docs"), navigateToNext: false);
        navMan.Uri.Is("http://localhost/?path=/story/examples-button--primary-button");

        navService.NavigateToNextDocsOrStory(new("story", "examples-button--primary-button"), navigateToNext: false);
        navMan.Uri.Is("http://localhost/?path=/story/examples-button--default-button");

        navService.NavigateToNextDocsOrStory(new("story", "examples-button--default-button"), navigateToNext: false);
        navMan.Uri.Is("http://localhost/?path=/docs/examples-button--docs");

        navService.NavigateToNextDocsOrStory(new("docs", "examples-button--docs"), navigateToNext: false);
        navMan.Uri.Is("http://localhost/?path=/docs/examples-button--docs");
    }
}
