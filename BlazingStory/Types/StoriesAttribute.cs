using System.Runtime.CompilerServices;

namespace BlazingStory.Types;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StoriesAttribute : Attribute
{
    #region Public Properties

    public string? Title { get; init; }

    #endregion Public Properties

    #region Internal Properties

    internal string FilePath { get; }

    #endregion Internal Properties

    #region Public Constructors

    public StoriesAttribute(string title, [CallerFilePath] string? callerFilePath = null)
    {
        this.Title = title;
        this.FilePath = callerFilePath ?? string.Empty;
    }

    #endregion Public Constructors
}
