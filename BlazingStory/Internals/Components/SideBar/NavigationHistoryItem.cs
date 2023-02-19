using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Components.SideBar;

internal class NavigationHistoryItem
{
    public required int Id;

    public required string Caption;

    public required NavigationTreeItemType Type;

    public required string NavigationPath;

    public required IEnumerable<string> Segments;
}
