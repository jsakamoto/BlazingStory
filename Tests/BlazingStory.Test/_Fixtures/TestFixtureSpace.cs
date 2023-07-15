using Toolbelt;

namespace BlazingStory.Test._Fixtures;

/// <summary>
/// A class that provides a working directory that contains the fixture test project files.
/// </summary>
internal class TestFixtureSpace : IDisposable
{
    /// <summary>
    /// The working directory that contains the fixture test project files.<br/>
    /// The working directory is created by copying the fixture directory into under the output folder of the test project.
    /// </summary>
    internal WorkDirectory WorkDir { get; }

    /// <summary>The full path of the BlazingStory project directory.</summary>
    internal string BlazingStoryProjDir { get; }

    /// <summary>The full path of the /Build/BlazingStory.targets file.</summary>
    internal string BlazingStoryTargetsPath { get; }

    /// <summary>
    /// A class that provides a working directory that contains the fixture test project files.
    /// </summary>
    internal TestFixtureSpace()
    {
        var solutionDir = FileIO.FindContainerDirToAncestor("*.sln");
        var fixtureDir = Path.Combine(solutionDir, "Tests", "Fixtures");
        var blazingStoryProjDir = Path.Combine(solutionDir, "BlazingStory");
        this.WorkDir = WorkDirectory.CreateCopyFrom(fixtureDir, file => file.Name is not "obj" and not "bin");
        this.BlazingStoryProjDir = Path.Combine(solutionDir, "BlazingStory");
        this.BlazingStoryTargetsPath = Path.Combine(this.BlazingStoryProjDir, "Build", "BlazingStory.targets");
    }

    /// <summary>
    /// Get the full path of the project directory of the specified test app project.
    /// </summary>
    internal string GetTestAppProjDir(string projectName) => Path.Combine(this.WorkDir, projectName);

    /// <summary>
    /// Create a global.json file with the specified parameters in the working directory.
    /// </summary>
    internal void CreateGlobalJson(string SDKVersion, string versionSuffix, bool allowPrerelease)
    {
        var globalJson = new DotNetSDKVersion(SDKVersion, versionSuffix, allowPrerelease).ToGlobalJsonText();
        File.WriteAllText(Path.Combine(this.WorkDir, "global.json"), globalJson);
    }

    public void Dispose()
    {
        this.WorkDir.Dispose();
    }
}
