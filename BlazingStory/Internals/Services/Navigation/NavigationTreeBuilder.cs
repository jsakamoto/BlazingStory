using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Services.Navigation;

/// <summary>
/// NavigationTreeBuilder builds a tree of <see cref="NavigationTreeItem" /> from a collection of
/// <see cref="StoryContainer" />.
/// </summary>
internal class NavigationTreeBuilder
{
    /// <summary>
    /// Build a tree of <see cref="NavigationTreeItem" /> from a collection of <see
    /// cref="StoryContainer" />.
    /// </summary>
    /// <param name="components">
    /// A collection of <see cref="StoryContainer" /> that is the source of the navigation item tree
    /// </param>
    /// <param name="expandedNavigationPath">
    /// A navigation path string to specify the tree item node that should be expanded (ex."/story/examples-button--primary")
    /// </param>
    /// <returns>
    /// </returns>
    internal NavigationTreeItem Build(IEnumerable<StoryContainer> components, string? expandedNavigationPath)
    {
        var root = new NavigationTreeItem { Type = NavigationItemType.Container };

        foreach (var component in components)
        {
            var segments = component.Title.Split('/');
            var componentNode = this.CreateOrGetNavigationTreeItem(root, pathSegments: Enumerable.Empty<string>(), segments);
            componentNode.Type = NavigationItemType.Component;

            var pathSegments = componentNode.PathSegments.Append(componentNode.Caption).ToArray();

            // Add a "Docs" node for the component
            var docsNode = new NavigationTreeItem
            {
                Type = NavigationItemType.Docs,
                NavigationPath = "/docs/" + NavigationPath.Create(component.Title, "Docs"),
                PathSegments = pathSegments,
                Caption = "Docs"
            };
            componentNode.SubItems.Add(docsNode);

            // Add "Story" nodes that live in the component
            var storyNodes = component.Stories
                .Select(story => new NavigationTreeItem
                {
                    Type = NavigationItemType.Story,
                    NavigationPath = "/story/" + story.NavigationPath,
                    PathSegments = pathSegments,
                    Caption = story.Name
                });
            componentNode.SubItems.AddRange(storyNodes);
        }

        root.SortSubItemsRecurse();

        if (!string.IsNullOrEmpty(expandedNavigationPath))
        {
            var expansionPath = new Stack<NavigationTreeItem>();
            if (FindExpansionPathTo(expansionPath, root, expandedNavigationPath))
            {
                foreach (var expansion in expansionPath) { expansion.Expanded = true; }
            }
        }

        root.SubItems.ForEach(story => story.Expanded = true);

        return root;
    }

    private static bool FindExpansionPathTo(Stack<NavigationTreeItem> expansionPath, NavigationTreeItem item, string expandedNavigationPath)
    {
        expansionPath.Push(item);

        if (item.NavigationPath == expandedNavigationPath)
        {
            return true;
        }

        foreach (var subItem in item.SubItems)
        {
            if (FindExpansionPathTo(expansionPath, subItem, expandedNavigationPath))
            {
                return true;
            }
        }

        expansionPath.Pop();
        return false;
    }

    private NavigationTreeItem CreateOrGetNavigationTreeItem(NavigationTreeItem item, IEnumerable<string> pathSegments, IEnumerable<string> segments)
    {
        var head = segments.First();
        var tails = segments.Skip(1);

        var subItem = item.SubItems.Find(sub => sub.Caption == head);

        if (subItem == null)
        {
            subItem = new NavigationTreeItem
            {
                Type = NavigationItemType.Container,
                PathSegments = pathSegments,
                Caption = head
            };
            item.SubItems.Add(subItem);
        }

        if (!tails.Any())
        {
            return subItem;
        }

        return this.CreateOrGetNavigationTreeItem(subItem, pathSegments.Append(head).ToArray(), tails);
    }
}
