using System.Text.RegularExpressions;

namespace BlazingStory.Test._Fixtures;

internal static class CsprojTools
{
    /// <summary>
    /// Rewrite the project file to change the target framework and the path to the
    /// BlazingStory.targets file.
    /// </summary>
    internal static void Rewrite(string projDir, string targetFramework, string blazingStoryTargetsPath)
    {
        // Find the path to the project file from the project directory.
        var projPath = Directory.EnumerateFiles(projDir, "*.csproj").Single();
        var projectContent = File.ReadAllText(projPath);

        // Replace the target framework specification in the project content with the pattern such
        // as "<TaregtFramework>net???</TargetFramework>" by Regular Expression.
        projectContent = Regex.Replace(projectContent, @"<TargetFramework>net[\d\.]+</TargetFramework>", $"<TargetFramework>{targetFramework}</TargetFramework>");
        projectContent = projectContent.Replace(@"""..\..\..\BlazingStory\Build\BlazingStory.targets""", @$"""{blazingStoryTargetsPath}""");

        // Save it.
        File.WriteAllText(projPath, projectContent);
    }
}
