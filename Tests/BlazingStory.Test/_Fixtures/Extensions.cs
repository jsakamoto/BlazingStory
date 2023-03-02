using BlazingStory.Internals.Models;

namespace BlazingStory.Test._Fixtures;

internal static class Extensions
{
    public static string Dump(this NavigationListItem item) => $"{item.Type} | {item.Caption} | {string.Join('/', item.Segments)}";

    public static IEnumerable<string> Dump(this IEnumerable<NavigationListItem> items) => items.Select(item => item.Dump());
}
