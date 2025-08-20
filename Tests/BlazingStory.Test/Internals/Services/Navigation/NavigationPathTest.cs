using BlazingStory.Internals.Services.Navigation;

namespace BlazingStory.Test.Internals.Services.Navigation;
internal class NavigationPathTest
{
    [Test]
    public void Create_Test()
    {
        NavigationPath.Create("Examples/Ui/Button").Is("examples-ui-button");
    }

    [Test]
    public void Create_with_Name_Test()
    {
        NavigationPath.Create("Examples/Ui/Button", "Default").Is("examples-ui-button--default");
    }

    [Test]
    public void Create_with_Name_including_symbol_Test()
    {
        NavigationPath.Create("Examples/Ui/Button's Stories", "Filled & Primary").Is("examples-ui-button-s-stories--filled-primary");
    }

    [Test]
    public void Create_with_Number_Starting_Folder_Test()
    {
        NavigationPath.Create("123Examples/Ui/Button").Is("123examples-ui-button");
    }

    [Test]
    public void Create_with_Hyphen_In_Folder_Test()
    {
        NavigationPath.Create("Some-Folder/Ui/Button").Is("some-folder-ui-button");
    }

    [Test]
    public void Create_with_Number_And_Hyphen_Test()
    {
        NavigationPath.Create("123-Test/Ui-Components/Button").Is("123-test-ui-components-button");
    }
}
