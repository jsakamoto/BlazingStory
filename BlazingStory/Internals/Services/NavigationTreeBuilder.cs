using BlazingStory.Internals.Models;

namespace BlazingStory.Internals.Services;

internal class NavigationTreeBuilder
{
    public NavigationTreeItem Build(IEnumerable<StoryContainer> storyContainers)
    {
        var root = new NavigationTreeItem { Type = NavigationTreeItemType.Container };
        foreach (var storyContainer in storyContainers)
        {
            var segments = storyContainer.Title.Split('/');
            var item = this.CreateOrGetNavigationTreeItem(root, segments);
            item.Type = NavigationTreeItemType.StoryCollection;
            var subItems = storyContainer.Stories
                .Select(story => new NavigationTreeItem
                {
                    Type = NavigationTreeItemType.Story,
                    Caption = story.Name
                });
            item.SubItems.AddRange(subItems);
        }

        return root;
    }

    private NavigationTreeItem CreateOrGetNavigationTreeItem(NavigationTreeItem item, IEnumerable<string> segments)
    {
        var head = segments.First();
        var tails = segments.Skip(1);

        var subItem = item.SubItems.Find(sub => sub.Caption == head);
        if (subItem == null)
        {
            subItem = new NavigationTreeItem
            {
                Type = NavigationTreeItemType.Container,
                Caption = head
            };
            item.SubItems.Add(subItem);
        }

        if (!tails.Any()) return subItem;

        return this.CreateOrGetNavigationTreeItem(subItem, tails);
    }
}
