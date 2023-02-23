namespace BlazingStory.Internals.Models;

internal class NavigationHistoryItem : INavigationPath
{
    public required int Id;

    public required string Caption;

    public required NavigationTreeItemType Type;

    public required string NavigationPath { get; init; }

    public required IEnumerable<string> Segments;
}
