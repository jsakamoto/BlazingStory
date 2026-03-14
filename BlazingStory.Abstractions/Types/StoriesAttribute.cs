using System.Runtime.CompilerServices;

namespace BlazingStory.Types;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StoriesAttribute : Attribute
{
    public string? Title { get; init; }

    internal string FilePath { get; }

    public StoriesAttribute(string title, [CallerFilePath] string? callerFilePath = null)
    {
        this.Title = title;
        this.FilePath = callerFilePath ?? string.Empty;
    }
}