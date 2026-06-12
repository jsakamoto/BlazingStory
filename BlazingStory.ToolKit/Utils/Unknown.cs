namespace BlazingStory.ToolKit.Utils;

/// <summary>
/// Represents an unknown or unresolvable value.
/// </summary>
public class Unknown
{
    /// <summary>
    /// The singleton instance representing an unknown value.
    /// </summary>
    public static readonly Unknown Value = new();

    /// <summary>
    /// Returns a dash ("-") to indicate an unknown value.
    /// </summary>
    public override string ToString() => "-";
}
