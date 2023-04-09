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

    public static NavigationListItem CreateFrom(int id, NavigationTreeItem treeItem)
    {
        return new NavigationListItem
        {
            Id = id,
            Type = treeItem.Type,
            Caption = treeItem.Caption,
            NavigationPath = treeItem.NavigationPath,
            Segments = treeItem.PathSegments
        };
    }
}
