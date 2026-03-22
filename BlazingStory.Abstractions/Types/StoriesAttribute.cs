using System.Runtime.CompilerServices;

namespace BlazingStory.Types;

/// <summary>
/// Marks a component class as a stories definition and assigns it a title for navigation.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StoriesAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the title used to group and display the stories in the navigation tree.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Gets the source file path of the caller that applied this attribute.
    /// </summary>
    internal string FilePath { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoriesAttribute"/> class.
    /// </summary>
    /// <param name="title">The title used to group and display the stories.</param>
    /// <param name="callerFilePath">The file path of the caller, automatically provided by the compiler.</param>
    public StoriesAttribute(string title, [CallerFilePath] string? callerFilePath = null)
    {
        this.Title = title;
        this.FilePath = callerFilePath ?? string.Empty;
    }
}