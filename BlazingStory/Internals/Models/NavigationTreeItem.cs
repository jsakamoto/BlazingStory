namespace BlazingStory.Internals.Models;

public class NavigationTreeItem : INavigationPath
{
    internal NavigationItemType Type { get; set; }

    internal string Caption { get; set; } = "";

    public string NavigationPath { get; set; } = "";

    internal IEnumerable<string> PathSegments { get; set; } = Enumerable.Empty<string>();

    internal bool Expanded { get; set; } = false;

    internal List<NavigationTreeItem> SubItems { get; } = new();

    internal NavigationTreeItem()
    {
    }

    internal IEnumerable<NavigationTreeItem> EnumAll()
    {
        yield return this;
        foreach (var item in this.SubItems.SelectMany(subItem => subItem.EnumAll()))
        {
            yield return item;
        }
    }

    internal bool IsExpandedAll => !this.SubItems.Any() || (this.Expanded && this.SubItems.All(subItem => subItem.IsExpandedAll));

    internal void ToggleSubItemsExpansion()
    {
        var expanded = !this.IsExpandedAll;
        this.SubItems.ForEach(item => item.ApplyExpansionRecursively(expanded));
    }

    internal void ApplyExpansionRecursively(bool expanded)
    {
        this.Expanded = expanded;
        this.SubItems.ForEach(item => item.ApplyExpansionRecursively(expanded));
    }

    internal void EnsureExpandedTo(NavigationTreeItem item) => this.EnsureExpandedToCore(item);

    private bool EnsureExpandedToCore(NavigationTreeItem item)
    {
        if (Object.ReferenceEquals(this, item)) return true;
        foreach (var subItem in this.SubItems)
        {
            if (subItem.EnsureExpandedToCore(item))
            {
                this.Expanded = true;
                return true;
            }
        }
        return false;
    }

    internal NavigationTreeItem? FindParentOf(NavigationTreeItem item)
    {
        foreach (var subItem in this.SubItems)
        {
            if (subItem == item) return this;
            var parent = subItem.FindParentOf(item);
            if (parent != null) return parent;
        }
        return null;
    }
}