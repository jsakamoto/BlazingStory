using System.Runtime.CompilerServices;

namespace BlazingStory.Types;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class CustomAttribute : Attribute
{
    public string? Title { get; init; }
    public CustomAttribute(string title, [CallerFilePath] string? callerFilePath = null)
    {
        this.Title = title;
        this.FilePath = callerFilePath ?? string.Empty;
    }
}