using BlazingStory.Internals.Models;

namespace BlazingStory.Test._Fixtures;

internal static class Extensions
{
    public static string Dump(this NavigationListItem item) => $"{item.Type} | {item.Caption} | {string.Join('/', item.Segments)}";

    public static IEnumerable<string> Dump(this IEnumerable<NavigationListItem> items) => items.Select(item => item.Dump());

    public static string Dump(this NavigationTreeItem item) => $"{item.Type} | {item.Caption} | {item.SubItems.Count}{(item.Expanded ? " | Expanded" : "")}";

    public static IEnumerable<string> Captions(this IEnumerable<NavigationTreeItem> items) => items.Select(item => item.Caption);
}
