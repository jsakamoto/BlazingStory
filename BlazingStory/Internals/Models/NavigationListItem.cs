namespace BlazingStory.Internals.Models;

public class NavigationListItem : INavigationPath
{
    public required int Id;

    public required string Caption;

    public required NavigationItemType Type;

    /// <summary>
    /// Gets a navigation path string for the item.<br/>
    /// (ex. "/story/example-button--primary", "/docs/example-button--docs")
    /// </summary>
    public required string NavigationPath { get; init; }

    public required IEnumerable<string> Segments;

    internal static NavigationListItem CreateFrom(int id, NavigationTreeItem treeItem)
    {
        return new NavigationListItem
        {
            Id = id,
            Type = treeItem.Type,
            Caption = treeItem.Caption,
            NavigationPath = treeItem.NavigationPath switch
            {
                "" => treeItem.SubItems.First(item => item.Type is NavigationItemType.Docs or NavigationItemType.Story).NavigationPath,
                _ => treeItem.NavigationPath
            },
            Segments = treeItem.PathSegments
        };
    }

    /// <summary>
    /// Compares the equality of two <see cref="NavigationListItem"/>, except the <see cref="Id"/> field.
    /// </summary>
    /// <param name="other">The other <see cref="NavigationListItem"/> to compare.</param>
    internal bool Equals(NavigationListItem? other)
    {
        if (other == null) return false;
        return this.NavigationPath == other.NavigationPath && this.Caption == other.Caption && this.Type == other.Type;
    }
}
