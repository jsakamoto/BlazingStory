using BlazingStory.Internals.Services.Docs;
using System.Reflection;

namespace BlazingStory.Test.Internals.Services.Docs;

/// <summary>
/// Integration test to demonstrate the fix for embedded resource name normalization
/// </summary>
internal class EmbeddedResourceNameNormalizationTest
{
    [Test] 
    public void Demonstrate_Resource_Name_Normalization_Fix()
    {
        // Given - a file path with problematic folder names
        var rootNamespace = "MyProject";
        var relativeFilePath = @"123Examples\Some-Folder\Button.stories.razor";

        // When - using the old approach (direct string joining)
        var oldResourceName = string.Join('.', relativeFilePath.Split('/', '\\').Prepend(rootNamespace));

        // And - using the new approach (proper normalization)
        var method = typeof(StoriesRazorSource).GetMethod("CreateEmbeddedResourceName", BindingFlags.NonPublic | BindingFlags.Static);
        var newResourceName = (string)method!.Invoke(null, new object[] { rootNamespace, relativeFilePath })!;

        // Then - they should be different, demonstrating the fix
        oldResourceName.Is("MyProject.123Examples.Some-Folder.Button.stories.razor");
        newResourceName.Is("MyProject._123Examples.Some_Folder.Button.stories.razor");

        // The new approach matches .NET's embedded resource naming conventions:
        // - Folders starting with numbers are prefixed with underscore
        // - Hyphens are converted to underscores
        Console.WriteLine($"Old: {oldResourceName}");
        Console.WriteLine($"New: {newResourceName}");
        Console.WriteLine("Fix applied: Resource names now match .NET embedded resource conventions");
    }

    [Test]
    public void NormalizeResourceNameSegment_Various_Cases_Test()
    {
        // Use reflection to access the private method
        var method = typeof(StoriesRazorSource).GetMethod("NormalizeResourceNameSegment", BindingFlags.NonPublic | BindingFlags.Static);

        // Test normal folder name
        var result1 = (string)method!.Invoke(null, new object[] { "Stories" })!;
        result1.Is("Stories");

        // Test folder starting with number  
        var result2 = (string)method!.Invoke(null, new object[] { "123Examples" })!;
        result2.Is("_123Examples");

        // Test folder with hyphens
        var result3 = (string)method!.Invoke(null, new object[] { "Some-Folder" })!;
        result3.Is("Some_Folder");

        // Test folder with multiple special characters
        var result4 = (string)method!.Invoke(null, new object[] { "UI-Components@Test" })!;
        result4.Is("UI_Components_Test");

        // Test file name with extension
        var result5 = (string)method!.Invoke(null, new object[] { "Button.stories.razor" })!;
        result5.Is("Button.stories.razor");
    }
}