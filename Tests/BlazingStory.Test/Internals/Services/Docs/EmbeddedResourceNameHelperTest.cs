using BlazingStory.Internals.Services.Docs;

namespace BlazingStory.Test.Internals.Services.Docs;

internal class EmbeddedResourceNameHelperTest
{
    [Test]
    public void CreateResourceName_NormalFolder_Test()
    {
        // Given
        var rootNamespace = "TestApp";
        var relativePath = "Stories/Button.stories.razor";
        
        // When
        var resourceName = EmbeddedResourceNameHelper.CreateResourceName(rootNamespace, relativePath);
        
        // Then
        resourceName.Is("TestApp.Stories.Button.stories.razor");
    }

    [Test]
    public void CreateResourceName_FolderStartingWithDigit_Test()
    {
        // Given
        var rootNamespace = "TestApp";
        var relativePath = "01-intro/Example.stories.razor";
        
        // When
        var resourceName = EmbeddedResourceNameHelper.CreateResourceName(rootNamespace, relativePath);
        
        // Then
        resourceName.Is("TestApp._01_intro.Example.stories.razor");
    }

    [Test]
    public void CreateResourceName_FolderWithHyphens_Test()
    {
        // Given
        var rootNamespace = "TestApp";
        var relativePath = "foo-bar/Example.stories.razor";
        
        // When
        var resourceName = EmbeddedResourceNameHelper.CreateResourceName(rootNamespace, relativePath);
        
        // Then
        resourceName.Is("TestApp.foo_bar.Example.stories.razor");
    }

    [Test]
    public void CreateResourceName_FolderWithSpaces_Test()
    {
        // Given
        var rootNamespace = "TestApp";
        var relativePath = "Test Folder/Example.stories.razor";
        
        // When
        var resourceName = EmbeddedResourceNameHelper.CreateResourceName(rootNamespace, relativePath);
        
        // Then
        resourceName.Is("TestApp.Test_Folder.Example.stories.razor");
    }

    [Test]
    public void CreateResourceName_FolderWithSpecialChars_Test()
    {
        // Given
        var rootNamespace = "TestApp";
        var relativePath = "special!@#$%chars/Example.stories.razor";
        
        // When
        var resourceName = EmbeddedResourceNameHelper.CreateResourceName(rootNamespace, relativePath);
        
        // Then
        resourceName.Is("TestApp.special_____chars.Example.stories.razor");
    }

    [Test]
    public void CreateResourceName_NestedFolders_Test()
    {
        // Given
        var rootNamespace = "TestApp";
        var relativePath = "01-intro/02-components/Button.stories.razor";
        
        // When
        var resourceName = EmbeddedResourceNameHelper.CreateResourceName(rootNamespace, relativePath);
        
        // Then
        resourceName.Is("TestApp._01_intro._02_components.Button.stories.razor");
    }

    [Test]
    public void CreateResourceName_FileWithSpecialChars_Test()
    {
        // Given
        var rootNamespace = "TestApp";
        var relativePath = "Stories/My-Button.stories.razor";
        
        // When
        var resourceName = EmbeddedResourceNameHelper.CreateResourceName(rootNamespace, relativePath);
        
        // Then
        resourceName.Is("TestApp.Stories.My-Button.stories.razor");
    }

    [Test]
    public void CreateResourceName_WindowsPaths_Test()
    {
        // Given
        var rootNamespace = "TestApp";
        var relativePath = "Stories\\01-intro\\Button.stories.razor";
        
        // When
        var resourceName = EmbeddedResourceNameHelper.CreateResourceName(rootNamespace, relativePath);
        
        // Then
        resourceName.Is("TestApp.Stories._01_intro.Button.stories.razor");
    }
}