using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Services.Navigation;

internal class NavigationTreeBuilder
{
    internal NavigationTreeItem Build(IEnumerable<StoryContainer> storyContainers, string? expandedNavigationPath)
    {
        var root = new NavigationTreeItem { Type = NavigationItemType.Container };
        foreach (var storyContainer in storyContainers)
        {
            var segments = storyContainer.Title.Split('/');
            var item = this.CreateOrGetNavigationTreeItem(root, pathSegments: Enumerable.Empty<string>(), segments);
            item.Type = NavigationItemType.Component;
            var pathSegments = item.PathSegments.Append(item.Caption).ToArray();
            var subItems = storyContainer.Stories
                .Select(story => new NavigationTreeItem
                {
                    Type = NavigationItemType.Story,
                    NavigationPath = story.NavigationPath,
                    PathSegments = pathSegments,
                    Caption = story.Name
                });
            item.SubItems.AddRange(subItems);
        }

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

        if (!tails.Any()) return subItem;

        return this.CreateOrGetNavigationTreeItem(subItem, pathSegments.Append(head).ToArray(), tails);
    }

    private static bool FindExpansionPathTo(Stack<NavigationTreeItem> expansionPath, NavigationTreeItem item, string expandedNavigationPath)
    {
        expansionPath.Push(item);
        if (item.NavigationPath == expandedNavigationPath) return true;
        foreach (var subItem in item.SubItems)
        {
            if (FindExpansionPathTo(expansionPath, subItem, expandedNavigationPath)) return true;
        }
        expansionPath.Pop();
        return false;
    }
}
