namespace BlazingStory.Types;

/// <summary>
/// Represents a node in a navigation tree that can be either:<br/>
/// - an item (leaf) identified by a <see cref="Title"/>, or<br/>
/// - a grouping node that contains <see cref="SubItems"/>.
/// </summary>
/// <remarks>
/// This type behaves like a simple discriminated union. The <see cref="Type"/> field indicates which
/// data is meaningful for the instance:
/// - <see cref="NodeType.Item"/>: <see cref="Title"/> is populated; <see cref="SubItems"/> is empty.
/// - <see cref="NodeType.SubItems"/>: <see cref="SubItems"/> contains children; <see cref="Title"/> is empty.
/// </remarks>
public class NavigationTreeOrderEntry
{
    /// <summary>
    /// The display title for an <see cref="NodeType.Item"/> node.
    /// For <see cref="NodeType.SubItems"/> nodes, this value is an empty string.
    /// </summary>
    public readonly string Title;

    /// <summary>
    /// The collection of child entries for an <see cref="NodeType.SubItems"/> node.
    /// For <see cref="NodeType.Item"/> nodes, this list is empty.
    /// </summary>
    public readonly IList<NavigationTreeOrderEntry> SubItems;

    /// <summary>
    /// Enumerates the possible kinds of nodes represented by <see cref="NavigationTreeOrderEntry"/>.
    /// </summary>
    public enum NodeType
    {
        /// <summary>
        /// A leaf entry that is represented by a <see cref="Title"/>.
        /// </summary>
        Item,

        /// <summary>
        /// A grouping entry that contains one or more child entries in <see cref="SubItems"/>.
        /// </summary>
        SubItems
    }

    /// <summary>
    /// Indicates the kind of this node and which field (<see cref="Title"/> or <see cref="SubItems"/>) is meaningful.
    /// </summary>
    public readonly NodeType Type;

    /// <summary>
    /// Initializes a new <see cref="NavigationTreeOrderEntry"/> as an <see cref="NodeType.Item"/> node.
    /// </summary>
    /// <param name="title">The non-empty display title for the item.</param>
    /// <remarks>
    /// The <see cref="SubItems"/> list is initialized as empty.
    /// </remarks>
    public NavigationTreeOrderEntry(string title)
    {
        this.Type = NodeType.Item;
        this.Title = title;
        this.SubItems = [];
    }

    /// <summary>
    /// Initializes a new <see cref="NavigationTreeOrderEntry"/> as a <see cref="NodeType.SubItems"/> node.
    /// </summary>
    /// <param name="subItems">The list of child entries to associate with this node.</param>
    /// <remarks>
    /// The provided list reference is stored as-is; it is not copied.
    /// </remarks>
    public NavigationTreeOrderEntry(IList<NavigationTreeOrderEntry> subItems)
    {
        this.Type = NodeType.SubItems;
        this.Title = "";
        this.SubItems = subItems;
    }

    /// <summary>
    /// Implicitly converts a <see cref="string"/> to an <see cref="NavigationTreeOrderEntry"/> item node.
    /// </summary>
    /// <param name="title">The display title for the item node.</param>
    /// <returns>
    /// A new <see cref="NavigationTreeOrderEntry"/> with <see cref="Type"/> set to <see cref="NodeType.Item"/>.
    /// </returns>
    public static implicit operator NavigationTreeOrderEntry(string title) => new(title);

    /// <summary>
    /// Implicitly converts an array of <see cref="NavigationTreeOrderEntry"/> elements
    /// to a grouping node containing those entries.
    /// </summary>
    /// <param name="subItems">The child entries to include in the grouping node.</param>
    /// <returns>
    /// A new <see cref="NavigationTreeOrderEntry"/> with <see cref="Type"/> set to <see cref="NodeType.SubItems"/>.
    /// </returns>
    public static implicit operator NavigationTreeOrderEntry(NavigationTreeOrderEntry[] subItems) => new(subItems);
}
