namespace BlazingStory.Internals.Types;

/// <summary>
/// Attribute to specify the project directory and root namespace of a Blazor WebAssembly project.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
public sealed class ProjectMetaDataAttribute : Attribute
{
    internal string ProjectDir { get; }

    internal string RootNamespace { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectMetaDataAttribute" /> class.
    /// </summary>
    /// <param name="projectDir">
    /// The project directory.
    /// </param>
    /// <param name="rootNamespace">
    /// The root namespace of the project.
    /// </param>
    // This is a positional argument
    public ProjectMetaDataAttribute(string projectDir, string rootNamespace)
    {
        this.ProjectDir = projectDir;
        this.RootNamespace = rootNamespace;
    }
}
