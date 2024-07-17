namespace BlazingStory.Internals.Types;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
public sealed class ProjectMetaDataAttribute : Attribute
{
    #region Internal Properties

    internal string ProjectDir { get; }

    internal string RootNamespace { get; }

    #endregion Internal Properties

    #region Public Constructors

    // This is a positional argument
    public ProjectMetaDataAttribute(string projectDir, string rootNamespace)
    {
        this.ProjectDir = projectDir;
        this.RootNamespace = rootNamespace;
    }

    #endregion Public Constructors
}
