namespace BlazingStory.Internals.Pages.TableOfContents;

/// <summary>
/// Represents a table-of-contents heading item.
/// </summary>
internal class TableOfContentsItem
{
    /// <summary>
    /// Gets the heading id.
    /// </summary>
    internal readonly string Id;

    /// <summary>
    /// Gets the heading text.
    /// </summary>
    internal readonly string Text;

    /// <summary>
    /// Gets the heading level.
    /// </summary>
    internal readonly int Level;

    /// <summary>
    /// Gets child heading items.
    /// </summary>
    internal readonly List<TableOfContentsItem> Children = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="TableOfContentsItem"/> class.
    /// </summary>
    /// <param name="id">The heading id.</param>
    /// <param name="text">The heading text.</param>
    /// <param name="level">The heading level.</param>
    internal TableOfContentsItem(string id, string text, int level)
    {
        this.Id = id;
        this.Text = text;
        this.Level = level;
    }
}