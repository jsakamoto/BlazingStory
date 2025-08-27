namespace BlazingStory.Types;

public class NavigationTreeOrderingBuilder
{
    public static readonly NavigationTreeOrderingBuilder N = new();

    public NavigationTreeOrdering[] this[params NavigationTreeOrdering[] items] => items;
}
