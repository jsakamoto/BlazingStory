using System.Text.RegularExpressions;

namespace BlazingStory.Internals.Services.Navigation;

internal class NavigationPath
{
    /// <summary>
    /// Create a navigation path from title and name (optional).<br/>
    /// If you provide the title "Examples/Ui/Button" and the name "Default", you will get "examples-ui-button--default".<br/>
    /// If you provide only the title "Examples/Ui/Button", you will get "examples-ui-button".
    /// </summary>
    /// <param name="title">A title to create the navigation path</param>
    /// <param name="name">A name to create the navigation path (optional)</param>
    /// <returns></returns>
    internal static string Create(string title, string name = "")
    {
        static string normalize(string value) => Regex.Replace(value, @"[^\w\p{IsCJKUnifiedIdeographs}]+", "-");
        return (normalize(title) + (string.IsNullOrEmpty(name) ? "" : ("--" + normalize(name)))).ToLower();
    }
}
