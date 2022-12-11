namespace BlazingStory.Internals.Models;

internal class NavigationTreeItem
{
    public NavigationTreeItemType Type { get; set; }

    public string Caption { get; set; } = "";

    public bool Expanded { get; set; }

    public List<NavigationTreeItem> SubItems { get; } = new();

    public NavigationTreeItem()
	{
	}
}