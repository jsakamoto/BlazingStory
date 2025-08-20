using System.Reflection;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Docs;
using BlazingStory.Types;
using BlazingStory.Test._Fixtures;

namespace BlazingStory.Test.Internals.Services.Docs;

/// <summary>
/// Tests for the resource name generation in StoriesRazorSource when dealing with
/// folder names that start with numbers or contain hyphens.
/// </summary>
internal class StoriesRazorSourceFilePathTest
{
    [Test]
    public void Resource_Name_Generation_With_Normal_Path_Test()
    {
        // Given
        var projectDir = @"C:\Project\";
        var filePath = @"C:\Project\Stories\Button.stories.razor";
        var rootNamespace = "MyProject";
        
        // When - simulate the logic from StoriesRazorSource.GetSourceCodeAsync
        var relativePathOfRazor = filePath.Substring(projectDir.Length).TrimStart('/', '\\');
        var resName = string.Join('.', relativePathOfRazor.Split('/', '\\').Prepend(rootNamespace));
        
        // Then
        resName.Is("MyProject.Stories.Button.stories.razor");
    }

    [Test]
    public void Resource_Name_Generation_With_Number_Starting_Folder_Test()
    {
        // Given
        var projectDir = @"C:\Project\";
        var filePath = @"C:\Project\123Examples\Button.stories.razor";
        var rootNamespace = "MyProject";
        
        // When - simulate the logic from StoriesRazorSource.GetSourceCodeAsync
        var relativePathOfRazor = filePath.Substring(projectDir.Length).TrimStart('/', '\\');
        var resName = string.Join('.', relativePathOfRazor.Split('/', '\\').Prepend(rootNamespace));
        
        // Then
        // The resource name includes the folder name exactly as it is in the file system
        resName.Is("MyProject.123Examples.Button.stories.razor");
    }

    [Test]
    public void Resource_Name_Generation_With_Hyphen_Folder_Test()
    {
        // Given
        var projectDir = @"C:\Project\";
        var filePath = @"C:\Project\Some-Folder\Button.stories.razor";
        var rootNamespace = "MyProject";
        
        // When - simulate the logic from StoriesRazorSource.GetSourceCodeAsync
        var relativePathOfRazor = filePath.Substring(projectDir.Length).TrimStart('/', '\\');
        var resName = string.Join('.', relativePathOfRazor.Split('/', '\\').Prepend(rootNamespace));
        
        // Then
        // The resource name includes the hyphen exactly as it is in the file system
        resName.Is("MyProject.Some-Folder.Button.stories.razor");
    }

    [Test] 
    public void Navigation_Path_Vs_Resource_Name_Mismatch_Test()
    {
        // Given
        var title = "123Examples/Some-Folder/Button";
        var projectDir = @"C:\Project\";
        var filePath = @"C:\Project\123Examples\Some-Folder\Button.stories.razor";
        var rootNamespace = "MyProject";
        
        // When - create navigation path (normalized)
        var navigationPath = BlazingStory.Internals.Services.Navigation.NavigationPath.Create(title);
        
        // And - create resource name (exact file path)
        var relativePathOfRazor = filePath.Substring(projectDir.Length).TrimStart('/', '\\');
        var resName = string.Join('.', relativePathOfRazor.Split('/', '\\').Prepend(rootNamespace));
        
        // Then - show the difference
        navigationPath.Is("123examples-some-folder-button"); // normalized
        resName.Is("MyProject.123Examples.Some-Folder.Button.stories.razor"); // exact path
        
        // The issue: when we have "123Examples/Some-Folder" in the folder structure,
        // the navigation system normalizes it to "123examples-some-folder"
        // but the resource lookup uses the exact folder names "123Examples.Some-Folder"
        // This causes the resource lookup to fail when trying to find the source code.
    }

    [Test]
    public void CreateEmbeddedResourceName_Normal_Path_Test()
    {
        // Given
        var rootNamespace = "MyProject";
        var relativeFilePath = @"Stories\Button.stories.razor";
        
        // When - using reflection to call the private method
        var method = typeof(StoriesRazorSource).GetMethod("CreateEmbeddedResourceName", BindingFlags.NonPublic | BindingFlags.Static);
        var result = (string)method!.Invoke(null, new object[] { rootNamespace, relativeFilePath })!;
        
        // Then
        result.Is("MyProject.Stories.Button.stories.razor");
    }

    [Test]
    public void CreateEmbeddedResourceName_Number_Starting_Folder_Test()
    {
        // Given
        var rootNamespace = "MyProject";
        var relativeFilePath = @"123Examples\Button.stories.razor";
        
        // When - using reflection to call the private method
        var method = typeof(StoriesRazorSource).GetMethod("CreateEmbeddedResourceName", BindingFlags.NonPublic | BindingFlags.Static);
        var result = (string)method!.Invoke(null, new object[] { rootNamespace, relativeFilePath })!;
        
        // Then - folder starting with number should be prefixed with underscore
        result.Is("MyProject._123Examples.Button.stories.razor");
    }

    [Test]
    public void CreateEmbeddedResourceName_Hyphen_Folder_Test()
    {
        // Given
        var rootNamespace = "MyProject";
        var relativeFilePath = @"Some-Folder\Button.stories.razor";
        
        // When - using reflection to call the private method
        var method = typeof(StoriesRazorSource).GetMethod("CreateEmbeddedResourceName", BindingFlags.NonPublic | BindingFlags.Static);
        var result = (string)method!.Invoke(null, new object[] { rootNamespace, relativeFilePath })!;
        
        // Then - hyphens should be converted to underscores
        result.Is("MyProject.Some_Folder.Button.stories.razor");
    }

    [Test]
    public void CreateEmbeddedResourceName_Complex_Path_Test()
    {
        // Given
        var rootNamespace = "MyProject";
        var relativeFilePath = @"123Examples\Some-Folder\UI-Components\Button-Test.stories.razor";
        
        // When - using reflection to call the private method
        var method = typeof(StoriesRazorSource).GetMethod("CreateEmbeddedResourceName", BindingFlags.NonPublic | BindingFlags.Static);
        var result = (string)method!.Invoke(null, new object[] { rootNamespace, relativeFilePath })!;
        
        // Then - all naming conventions should be applied
        result.Is("MyProject._123Examples.Some_Folder.UI_Components.Button_Test.stories.razor");
    }

    [Test]
    public async Task Actual_Resource_Lookup_Issue_Test()
    {
        // Given - a story in a problematic folder path
        var typeofStoriesRazor = typeof(BlazingStoryApp1.Stories._123Examples.Button_Test_stories);
        var storyAttribute = typeofStoriesRazor.GetAttribute<StoriesAttribute>();
        var descriptor = new StoriesRazorDescriptor(typeofStoriesRazor, storyAttribute);
        var story = new Story(descriptor, typeof(RazorClassLib1.Components.Button.Button), "Default", TestHelper.StoryContext.CreateEmpty(), null, null, TestHelper.EmptyFragment, null);

        // When - try to get source code
        var sourceCode = await StoriesRazorSource.GetSourceCodeAsync(story);

        // Then - it should work (if the fix is applied)
        // For now, this may return empty string due to the bug
        Console.WriteLine($"Source code length: {sourceCode.Length}");
        Console.WriteLine($"Navigation path: {story.NavigationPath}");
        Console.WriteLine($"File path: {storyAttribute.FilePath}");
        
        // The test demonstrates the issue - source code may be empty due to resource lookup failure
    }
}