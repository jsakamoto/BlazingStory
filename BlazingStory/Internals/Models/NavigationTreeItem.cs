namespace BlazingStory.Internals.Models;

public class NavigationTreeItem
{
    internal NavigationTreeItemType Type { get; set; }

    internal string Caption { get; set; } = "";

    internal bool Expanded { get; set; } = true;

    internal List<NavigationTreeItem> SubItems { get; } = new();

    internal NavigationTreeItem()
    {
    }
}