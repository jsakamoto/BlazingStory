namespace BlazingStory.Internals.Types;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
public sealed class ProjectMetaDataAttribute : Attribute
{
    // This is a positional argument
    public ProjectMetaDataAttribute(string projectDir, string rootNamespace)
    {
        this.ProjectDir = projectDir;
        this.RootNamespace = rootNamespace;
    }

    internal string ProjectDir { get; }

    internal string RootNamespace { get; }
}
