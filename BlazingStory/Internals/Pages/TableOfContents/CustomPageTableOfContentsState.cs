namespace BlazingStory.Internals.Pages.TableOfContents;

/// <summary>
/// Holds table-of-contents state for the active custom page.
/// </summary>
internal class CustomPageTableOfContentsState
{
    internal int MinHeadingLevel { get; private set; } = TableOfContentsDefaults.MinHeadingLevel;

    internal int MaxHeadingLevel { get; private set; } = TableOfContentsDefaults.MaxHeadingLevel;

    /// <summary>
    /// Gets the current table-of-contents items.
    /// </summary>
    internal IReadOnlyList<TableOfContentsItem> Items { get; private set; } = [];

    /// <summary>
    /// Occurs when table-of-contents items change.
    /// </summary>
    internal event EventHandler? Changed;

    /// <summary>
    /// Sets table-of-contents items and notifies subscribers.
    /// </summary>
    /// <param name="items">The table-of-contents items.</param>
    internal void SetItems(IReadOnlyList<TableOfContentsItem> items)
    {
        this.Items = items;
        this.Changed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Sets heading level range used for TOC generation.
    /// </summary>
    /// <param name="minHeadingLevel">The minimum heading level.</param>
    /// <param name="maxHeadingLevel">The maximum heading level.</param>
    internal void SetHeadingLevelRange(int minHeadingLevel, int maxHeadingLevel)
    {
        var (normalizedMin, normalizedMax) = TableOfContentsDefaults.NormalizeHeadingLevelRange(minHeadingLevel, maxHeadingLevel);
        this.MinHeadingLevel = normalizedMin;
        this.MaxHeadingLevel = normalizedMax;
    }
}