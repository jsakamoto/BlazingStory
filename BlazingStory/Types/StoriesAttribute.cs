using System.Runtime.CompilerServices;

namespace BlazingStory.Types;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StoriesAttribute : Attribute
{
    public StoriesAttribute(string title, [CallerFilePath] string? callerFilePath = null)
    {
        this.Title = title;
        this.FilePath = callerFilePath ?? string.Empty;
    }

    public string? Title { get; init; }

    internal string FilePath { get; }
}
