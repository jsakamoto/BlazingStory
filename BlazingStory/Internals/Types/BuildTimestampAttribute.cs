namespace BlazingStory.Internals.Types;

/// <summary>
/// Represents the month when this package was built.<br/>
/// This attribute is used to display the build month in the Release page.
/// This attribute will be applied to the BlazingStory assembly through the build process (see also the "BlazingStory.csproj" MSBuild script).
/// </summary>
internal class BuildTimestampAttribute : Attribute
{
    /// <summary>
    /// Gets the text represents the month when this package was built.<br/>
    /// (ex."August 2023")
    /// </summary>
    internal string BuildMonthText { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildTimestampAttribute"/> class.
    /// </summary>
    /// <param name="buildMonthText">The text represents the month when this package was built.</param>
    public BuildTimestampAttribute(string buildMonthText)
    {
        this.BuildMonthText = buildMonthText;
    }
}
