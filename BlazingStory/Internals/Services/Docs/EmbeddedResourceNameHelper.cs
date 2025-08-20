using System.Text.RegularExpressions;

namespace BlazingStory.Internals.Services.Docs;

/// <summary>
/// Helper class to convert file paths to MSBuild-compatible embedded resource names.
/// MSBuild follows C# identifier naming rules when creating resource names from file paths.
/// </summary>
internal static class EmbeddedResourceNameHelper
{
    /// <summary>
    /// Converts a file path to an embedded resource name using the same rules as MSBuild.
    /// </summary>
    /// <param name="rootNamespace">The root namespace of the project</param>
    /// <param name="relativePath">The relative path of the file</param>
    /// <returns>The embedded resource name</returns>
    internal static string CreateResourceName(string rootNamespace, string relativePath)
    {
        // Split the path into segments - folders and filename
        var pathSegments = relativePath.Split('/', '\\');

        // Transform all segments except the last one (filename) to be valid C# identifiers
        var transformedSegments = pathSegments
            .Take(pathSegments.Length - 1)
            .Select(TransformToValidIdentifier)
            .Concat([pathSegments.LastOrDefault()])
            .Where(segment => !string.IsNullOrEmpty(segment)); // Remove empty segments

        // Join with root namespace
        return string.Join('.', new[] { rootNamespace }.Concat(transformedSegments));
    }

    /// <summary>
    /// Transforms a string to be a valid C# identifier following MSBuild rules.
    /// </summary>
    /// <param name="input">The input string</param>
    /// <returns>A valid C# identifier</returns>
    private static string TransformToValidIdentifier(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        // Replace all non-alphanumeric characters with underscores
        var result = Regex.Replace(input, @"[^a-zA-Z0-9]", "_");

        // If the result starts with a digit, prefix with underscore
        return char.IsDigit(result[0]) ? "_" + result : result;
    }
}