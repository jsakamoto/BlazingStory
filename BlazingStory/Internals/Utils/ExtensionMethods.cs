namespace BlazingStory.Internals.Utils;

internal static class ExtensionMethods
{
    public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
    {
        foreach (var item in values) action(item);
    }
}
