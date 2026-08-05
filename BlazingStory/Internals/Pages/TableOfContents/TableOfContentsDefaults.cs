namespace BlazingStory.Internals.Pages.TableOfContents;

/// <summary>
/// Provides default table-of-contents settings.
/// </summary>
internal static class TableOfContentsDefaults
{
    /// <summary>
    /// The minimum included heading level.
    /// </summary>
    internal const int MinHeadingLevel = 1;

    /// <summary>
    /// The maximum included heading level.
    /// </summary>
    internal const int MaxHeadingLevel = 4;

    /// <summary>
    /// The smallest allowed heading level.
    /// </summary>
    internal const int SmallestHeadingLevel = 1;

    /// <summary>
    /// The largest allowed heading level.
    /// </summary>
    internal const int LargestHeadingLevel = 6;

    /// <summary>
    /// Normalizes a heading level into the supported range.
    /// </summary>
    /// <param name="level">The heading level.</param>
    /// <returns>The normalized heading level.</returns>
    internal static int NormalizeHeadingLevel(int level)
    {
        return Math.Clamp(level, SmallestHeadingLevel, LargestHeadingLevel);
    }

    /// <summary>
    /// Normalizes a heading level range into the supported range.
    /// </summary>
    /// <param name="minHeadingLevel">The minimum heading level.</param>
    /// <param name="maxHeadingLevel">The maximum heading level.</param>
    /// <returns>The normalized heading level range.</returns>
    internal static (int MinHeadingLevel, int MaxHeadingLevel) NormalizeHeadingLevelRange(int minHeadingLevel, int maxHeadingLevel)
    {
        var min = NormalizeHeadingLevel(minHeadingLevel);
        var max = NormalizeHeadingLevel(maxHeadingLevel);
        return min <= max ? (min, max) : (max, min);
    }

    /// <summary>
    /// Determines whether a heading level is included in the table of contents.
    /// </summary>
    /// <param name="level">The heading level.</param>
    /// <param name="minHeadingLevel">The minimum included heading level.</param>
    /// <param name="maxHeadingLevel">The maximum included heading level.</param>
    /// <returns><see langword="true"/> when included; otherwise, <see langword="false"/>.</returns>
    internal static bool IsIncludedHeadingLevel(int level, int minHeadingLevel, int maxHeadingLevel)
    {
        var (normalizedMin, normalizedMax) = NormalizeHeadingLevelRange(minHeadingLevel, maxHeadingLevel);
        return level >= normalizedMin && level <= normalizedMax;
    }
}

/// <summary>
/// Represents a source heading used to build table-of-contents entries.
/// </summary>
/// <param name="Text">The heading text.</param>
/// <param name="Level">The heading level.</param>
/// <param name="Id">The optional heading id.</param>
internal readonly record struct TableOfContentsSourceHeading(string Text, int Level, string? Id = null);