namespace BlazingStory.Internals.Models;

public class NavigationTreeItem : INavigationPath
{
    internal NavigationItemType Type { get; set; }

    internal string Caption { get; set; } = "";

    /// <summary>
    /// Gets a navigation path string for the item.<br/>
    /// (ex. "/story/example-button--primary", "/docs/example-button--docs")
    /// </summary>
    public string NavigationPath { get; init; } = "";

    internal IEnumerable<string> PathSegments { get; set; } = Enumerable.Empty<string>();

    internal bool Expanded { get; set; } = false;

    internal List<NavigationTreeItem> SubItems { get; } = new();

    internal NavigationTreeItem()
    {
    }

    /// <summary>
    /// Sorts sub items recursively by its caption, except stories.
    /// </summary>
    internal void SortSubItemsRecurse()
    {
        if (this.SubItems.Count == 0) return;
        if (this.SubItems.First().Type is not NavigationItemType.Container and not NavigationItemType.Component) return;

        this.SubItems.Sort((a, b) => a.Caption.CompareTo(b.Caption));
        this.SubItems.ForEach(item => item.SortSubItemsRecurse());
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