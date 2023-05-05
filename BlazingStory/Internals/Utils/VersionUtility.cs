using System.Reflection;

namespace BlazingStory.Internals.Utils;

/// <summary>
/// Utility class for obtaining version information.
/// </summary>
internal static class VersionUtility
{
    /// <summary>
    /// Get the version text of the assembly.<br/>
    /// (ex. "1.0.0-preview.2.3")
    /// </summary>
    public static string GetVersionText()
    {
        var assembly = typeof(VersionUtility).Assembly;
        var verInfo = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        return verInfo?.InformationalVersion ?? assembly.GetName().Version?.ToString() ?? "";
    }
}
