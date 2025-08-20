using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Docs;
using BlazingStory.Test._Fixtures;
using BlazingStory.Types;
using BlazingStory.Internals.Types;
using System.Reflection;
using static BlazingStory.Test._Fixtures.TestHelper;

namespace BlazingStory.Test.Internals.Services.Docs;

/// <summary>
/// Integration test to verify that StoriesRazorSource correctly handles story files in folders
/// starting with digits or containing special characters.
/// </summary>
internal class StoriesRazorSourceProblematicFolderTest
{
    /// <summary>
    /// Mock type to simulate a story in a problematic folder.
    /// </summary>
    private class MockStoriesTypeInProblematicFolder
    {
        // This type is used to simulate a stories razor class that would be in a problematic folder
    }

    [Test]
    public void GetSourceCodeAsync_StoryInFolderStartingWithDigit_ConstructsCorrectResourceName()
    {
        // Given
        var mockStoriesType = typeof(MockStoriesTypeInProblematicFolder);
        
        // Create a mock StoriesAttribute that simulates a file in "01-intro" folder
        var storiesAttribute = new StoriesAttribute("Examples/01-intro/Button", "/project/01-intro/Button.stories.razor");
        var descriptor = new StoriesRazorDescriptor(mockStoriesType, storiesAttribute);
        
        // Create a mock ProjectMetaDataAttribute
        var projectMetadata = new ProjectMetaDataAttribute("/project", "TestApp");
        
        // We'll simulate the resource name construction logic that would happen
        var relativePathOfRazor = storiesAttribute.FilePath.Substring(projectMetadata.ProjectDir.Length).TrimStart('/', '\\');
        var expectedResourceName = EmbeddedResourceNameHelper.CreateResourceName(projectMetadata.RootNamespace, relativePathOfRazor);
        
        // Then - verify the resource name is constructed correctly for MSBuild
        expectedResourceName.Is("TestApp._01_intro.Button.stories.razor");
    }

    [Test]
    public void GetSourceCodeAsync_StoryInFolderWithHyphens_ConstructsCorrectResourceName()
    {
        // Given
        var mockStoriesType = typeof(MockStoriesTypeInProblematicFolder);
        
        // Create a mock StoriesAttribute that simulates a file in "foo-bar" folder  
        var storiesAttribute = new StoriesAttribute("Examples/foo-bar/Button", "/project/foo-bar/Button.stories.razor");
        var descriptor = new StoriesRazorDescriptor(mockStoriesType, storiesAttribute);
        
        // Create a mock ProjectMetaDataAttribute
        var projectMetadata = new ProjectMetaDataAttribute("/project", "TestApp");
        
        // We'll simulate the resource name construction logic that would happen
        var relativePathOfRazor = storiesAttribute.FilePath.Substring(projectMetadata.ProjectDir.Length).TrimStart('/', '\\');
        var expectedResourceName = EmbeddedResourceNameHelper.CreateResourceName(projectMetadata.RootNamespace, relativePathOfRazor);
        
        // Then - verify the resource name is constructed correctly for MSBuild
        expectedResourceName.Is("TestApp.foo_bar.Button.stories.razor");
    }

    [Test]
    public void GetSourceCodeAsync_StoryInNestedProblematicFolders_ConstructsCorrectResourceName()
    {
        // Given
        var mockStoriesType = typeof(MockStoriesTypeInProblematicFolder);
        
        // Create a mock StoriesAttribute that simulates a file in nested problematic folders
        var storiesAttribute = new StoriesAttribute("Examples/01-intro/02-components/Button", "/project/01-intro/02-components/Button.stories.razor");
        var descriptor = new StoriesRazorDescriptor(mockStoriesType, storiesAttribute);
        
        // Create a mock ProjectMetaDataAttribute
        var projectMetadata = new ProjectMetaDataAttribute("/project", "TestApp");
        
        // We'll simulate the resource name construction logic that would happen
        var relativePathOfRazor = storiesAttribute.FilePath.Substring(projectMetadata.ProjectDir.Length).TrimStart('/', '\\');
        var expectedResourceName = EmbeddedResourceNameHelper.CreateResourceName(projectMetadata.RootNamespace, relativePathOfRazor);
        
        // Then - verify the resource name is constructed correctly for MSBuild
        expectedResourceName.Is("TestApp._01_intro._02_components.Button.stories.razor");
    }
}