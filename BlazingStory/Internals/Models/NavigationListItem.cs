namespace BlazingStory.Internals.Models;

internal class NavigationListItem : INavigationPath
{
    public required int Id;

    public required string Caption;

    public required NavigationItemType Type;

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
