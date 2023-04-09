namespace BlazingStory.Internals.Services.Navigation;

internal class NavigationPath
{
    public static string Create(string title, string name)
    {
        return (title.Replace("/", "-") + "--" + name).ToLower().Replace(' ', '-');
    }
}
