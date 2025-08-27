namespace BlazingStory.Types;

public class NavigationTreeOrdering
{
    public readonly string Title;

    public readonly IList<NavigationTreeOrdering> SubItems;

    public enum NodeType { Item, SubItems }

    public readonly NodeType Type;

    public NavigationTreeOrdering(string title)
    {
        this.Type = NodeType.Item;
        this.Title = title;
        this.SubItems = [];
    }

    public NavigationTreeOrdering(IList<NavigationTreeOrdering> subItems)
    {
        this.Type = NodeType.SubItems;
        this.Title = "";
        this.SubItems = subItems;
    }

    public static implicit operator NavigationTreeOrdering(string title) => new(title);

    public static implicit operator NavigationTreeOrdering(NavigationTreeOrdering[] subItems) => new(subItems);
}
