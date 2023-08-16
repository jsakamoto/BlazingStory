using System.Reflection;
using System.Text.RegularExpressions;

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
    public static string GetVersionText() => typeof(VersionUtility).Assembly.GetVersionText();

    /// <summary>
    /// Get the version text of the assembly.<br/>
    /// (ex. "1.0.0-preview.2.3")
    /// </summary>
    public static string GetVersionText(this Assembly assembly)
    {
        var verInfo = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        return verInfo?.InformationalVersion ?? assembly.GetName().Version?.ToString() ?? "";
    }

    /// <summary>
    /// Get the more human readble version text of the assembly.<br/>
    /// (ex. "1.0 Preview 2.3")
    /// </summary>
    public static string GetFormattedVersionText(this Assembly assembly)
    {
        var versionText = assembly.GetVersionText();

        var m1 = Regex.Match(versionText, @"^(?<version>\d+(\.\d+(\.\d+(\.\d+)?)?)?)([ \-]+(?<suffix>.*)?)?$");
        var version = Version.Parse(m1.Groups["version"].Value);
        var i = new[] { version.Major, version.Minor, version.Build, version.Revision }
            .Select((num, index) => (num, index))
            .Reverse()
            .First(x => x.num > 0)
            .index;
        var formattedVersionText = version.ToString(i + 1) + (i == 0 ? ".0" : "");

        if (string.IsNullOrEmpty(m1.Groups["suffix"].Value)) return formattedVersionText;

        var m2 = Regex.Match(m1.Groups["suffix"].Value, @"^(?<suffix>[^\d\.]*)(\.(?<revision>[\d\.]*\d+))?$");
        var suffix = m2.Groups["suffix"].Value;
        var revision = m2.Groups["revision"].Value;

        // Capitalize the first letter of the suffix.
        if (!string.IsNullOrEmpty(suffix)) suffix = char.ToUpper(suffix[0]) + suffix.Substring(1);

        return $"{formattedVersionText} {suffix} {revision}".Trim();
    }
}
