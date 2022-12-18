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

    internal bool IsExpandedAll => !this.SubItems.Any() || (this.Expanded && this.SubItems.All(subItem => subItem.IsExpandedAll));

    internal void ToggleSubItemsExpansion()
    {
        var expanded = !this.IsExpandedAll;
        this.SubItems.ForEach(item => item.ApplyExpansionRecursively(expanded));
    }

    private void ApplyExpansionRecursively(bool expanded)
    {
        this.Expanded = expanded;
        this.SubItems.ForEach(item => item.ApplyExpansionRecursively(expanded));
    }
}