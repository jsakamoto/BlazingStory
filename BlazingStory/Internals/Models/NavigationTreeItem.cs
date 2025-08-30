using BlazingStory.Types;

namespace BlazingStory.Internals.Models;

public class NavigationTreeItem : INavigationPath
{
    internal NavigationItemType Type { get; set; }

    internal string Caption { get; set; } = "";

    /// <summary>
    /// Gets a navigation path string for the item.<br/>
    /// (ex. "/story/example-button--primary", "/docs/example-button--docs")
    /// </summary>
    public string NavigationPath { get; set; } = "";

    internal IEnumerable<string> PathSegments { get; set; } = Enumerable.Empty<string>();

    internal bool Expanded { get; set; } = false;

    internal List<NavigationTreeItem> SubItems { get; } = new();

    internal NavigationTreeItem()
    {
    }

    /// <summary>
    /// Recursively sorts the sub-items of the current navigation tree item based on the specified custom ordering rules.
    /// </summary>
    /// <remarks>This method processes the sub-items of the current navigation tree item in the following manner:
    /// <list type="bullet">
    /// <item>Items of type <see cref="NavigationItemType.Docs"/> or <see cref="NavigationItemType.Story"/> are preserved in their default order and placed before other item types.</item>
    /// <item>Items matching the custom ordering rules are added to the sorted list in the specified order. If a custom ordering specifies sub-items, those sub-items are recursively sorted using the same logic.</item>
    /// <item>Any remaining items not covered by the custom ordering rules are sorted using the default comparer (<see cref="NavigationTreeItemComparer.Instance"/>) and appended to the sorted list. </item>
    /// </list>
    /// After sorting, the sub-items of the current navigation tree item are replaced with the newly sorted list.</remarks>
    /// <param name="customOrderings">A list of <see cref="NavigationTreeOrdering"/> objects that define the custom ordering rules for sorting. Each ordering specifies the desired sequence of items and sub-items within the navigation tree.</param>
    internal void SortSubItemsRecurse(IList<NavigationTreeOrdering> customOrderings)
    {
        static bool filterStories(NavigationTreeItem item) => item.Type is NavigationItemType.Docs or NavigationItemType.Story;
        var itemSourceSet = this.SubItems.Where(item => !filterStories(item)).ToDictionary(item => item.Caption, item => item);
        var sortedItems = new List<NavigationTreeItem>(capacity: this.SubItems.Count);

        // Keep the default ordering for docs and stories, placing them before other item types.
        sortedItems.AddRange(this.SubItems.Where(filterStories));

        // Sort the navigation tree items according to the custom ordering specifications.
        for (var i = 0; i < customOrderings.Count; i++)
        {
            var request = customOrderings[i];
            if (request.Type != NavigationTreeOrdering.NodeType.Item) continue;
            if (itemSourceSet.TryGetValue(request.Title, out var item))
            {
                sortedItems.Add(item);
                itemSourceSet.Remove(request.Title);

                var nextIsSubItems = i + 1 < customOrderings.Count && customOrderings[i + 1].Type == NavigationTreeOrdering.NodeType.SubItems;
                var subCustomOrderings = nextIsSubItems ? customOrderings[i + 1].SubItems : [];

                // Sort the sub items recursively
                item.SortSubItemsRecurse(subCustomOrderings);

                if (nextIsSubItems) i++;
            }
        }

        // Sort remains recursively
        var sortedRemains = itemSourceSet.Values.Order(comparer: NavigationTreeItemComparer.Instance).ToArray();
        foreach (var item in sortedRemains) item.SortSubItemsRecurse([]);
        sortedItems.AddRange(sortedRemains);

        // Replace the sub items
        for (var i = 0; i < sortedItems.Count; i++) this.SubItems[i] = sortedItems[i];
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

    internal class NavigationTreeItemComparer : IComparer<NavigationTreeItem>
    {
        internal static readonly NavigationTreeItemComparer Instance = new();

        int IComparer<NavigationTreeItem>.Compare(NavigationTreeItem? a, NavigationTreeItem? b)
        {
            if (a is null || b is null) return 0;
            if (a.Type == NavigationItemType.CustomPage && b.Type != NavigationItemType.CustomPage) return 1;
            if (a.Type != NavigationItemType.CustomPage && b.Type == NavigationItemType.CustomPage) return -1;
            return a.Caption.CompareTo(b.Caption);
        }
    }
}