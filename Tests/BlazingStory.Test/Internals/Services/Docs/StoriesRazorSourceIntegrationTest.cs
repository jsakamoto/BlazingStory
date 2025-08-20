using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.Docs;
using BlazingStory.Internals.Services.Navigation;
using BlazingStory.Types;
using System.Reflection;

namespace BlazingStory.Test.Internals.Services.Docs;

/// <summary>
/// End-to-end integration test for the embedded resource name fix
/// </summary>
internal class StoriesRazorSourceIntegrationTest
{
    [Test]
    public void End_To_End_Resource_Name_Generation_Test()
    {
        // Given - simulate story metadata for a problematic folder structure
        var storyTitle = "123Examples/Some-Folder/Button";
        var filePath = @"C:\MyProject\Stories\123Examples\Some-Folder\Button.stories.razor";
        var projectDir = @"C:\MyProject\";
        var rootNamespace = "MyProject.Stories";

        // When - create navigation path (this is how the story is identified in the UI)
        var navigationPath = NavigationPath.Create(storyTitle);

        // And - simulate the resource name generation logic from StoriesRazorSource
        var relativePathOfRazor = filePath.Substring(projectDir.Length).TrimStart('/', '\\');
        
        // Old approach (broken)
        var oldResourceName = string.Join('.', relativePathOfRazor.Split('/', '\\').Prepend(rootNamespace));
        
        // New approach (fixed)
        var method = typeof(StoriesRazorSource).GetMethod("CreateEmbeddedResourceName", BindingFlags.NonPublic | BindingFlags.Static);
        var newResourceName = (string)method!.Invoke(null, new object[] { rootNamespace, relativePathOfRazor })!;

        // Then - verify the results
        Console.WriteLine($"Story Title: {storyTitle}");
        Console.WriteLine($"Navigation Path: {navigationPath}");
        Console.WriteLine($"File Path: {filePath}");
        Console.WriteLine($"Old Resource Name: {oldResourceName}");
        Console.WriteLine($"New Resource Name: {newResourceName}");
        
        // Assertions
        navigationPath.Is("123examples-some-folder-button");
        oldResourceName.Is("MyProject.Stories.Stories.123Examples.Some-Folder.Button.stories.razor");
        newResourceName.Is("MyProject.Stories.Stories._123Examples.Some_Folder.Button.stories.razor");

        // The key difference: the new approach normalizes folder names to match .NET conventions
        newResourceName.ShouldNotBe(oldResourceName, "Fixed resource name should differ from broken one");
        newResourceName.ShouldContain("_123Examples", "Folders starting with numbers should be prefixed with underscore");  
        newResourceName.ShouldContain("Some_Folder", "Hyphens should be converted to underscores");
    }

    [Test]
    public void Verify_Resource_Name_Follows_CSharp_Identifier_Rules_Test()
    {
        var testCases = new[]
        {
            ("123Test", "_123Test"),
            ("9Folder", "_9Folder"), 
            ("Some-Folder", "Some_Folder"),
            ("Test@Name", "Test_Name"),
            ("A+B", "A_B"),
            ("Valid_Name", "Valid_Name"),
            ("AlreadyValid", "AlreadyValid")
        };

        var method = typeof(StoriesRazorSource).GetMethod("NormalizeResourceNameSegment", BindingFlags.NonPublic | BindingFlags.Static);

        foreach (var (input, expected) in testCases)
        {
            var result = (string)method!.Invoke(null, new object[] { input })!;
            result.Is(expected);
            
            // Verify that the result is a valid C# identifier part (except for dots and file extensions)
            if (!input.Contains('.'))
            {
                ShouldBeValidCSharpIdentifierPart(result);
            }
        }
    }

    private static void ShouldBeValidCSharpIdentifierPart(string identifier)
    {
        // Basic validation that the identifier follows C# naming rules for embedded resources
        identifier.ShouldNotBeNullOrEmpty();
        
        // First character should be letter or underscore
        var firstChar = identifier[0];
        (char.IsLetter(firstChar) || firstChar == '_').ShouldBeTrue($"First character '{firstChar}' should be letter or underscore");
        
        // All characters should be letters, digits, or underscores
        foreach (char c in identifier)
        {
            (char.IsLetterOrDigit(c) || c == '_').ShouldBeTrue($"Character '{c}' should be letter, digit, or underscore");
        }
    }

    [Test]
    public void Performance_Test_Resource_Name_Generation()
    {
        // Given - multiple test cases
        var testCases = new[]
        {
            ("MyApp", "Stories/Button.stories.razor"),
            ("MyApp", "123Examples/Button.stories.razor"),
            ("MyApp", "Some-Folder/Rating.stories.razor"),
            ("MyApp", "123-Test/UI-Components/Complex-Button.stories.razor"),
            ("MyApp", "9Test/8Components/7Folder/Button.stories.razor")
        };

        var method = typeof(StoriesRazorSource).GetMethod("CreateEmbeddedResourceName", BindingFlags.NonPublic | BindingFlags.Static);
        
        // When - perform many resource name generations
        var startTime = DateTime.Now;
        const int iterations = 1000;
        
        for (int i = 0; i < iterations; i++)
        {
            foreach (var (rootNamespace, path) in testCases)
            {
                var result = (string)method!.Invoke(null, new object[] { rootNamespace, path })!;
                result.ShouldNotBeNullOrEmpty();
            }
        }
        
        var elapsed = DateTime.Now - startTime;
        
        // Then - should be reasonably fast (less than 1 second for 5000 operations)
        elapsed.TotalSeconds.ShouldBeLessThan(1.0, $"Resource name generation took {elapsed.TotalSeconds} seconds for {iterations * testCases.Length} operations");
        
        Console.WriteLine($"Generated {iterations * testCases.Length} resource names in {elapsed.TotalMilliseconds:F2}ms");
        Console.WriteLine($"Average: {elapsed.TotalMilliseconds / (iterations * testCases.Length):F4}ms per operation");
    }
}