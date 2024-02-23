namespace BlazingStory.Internals.Utils;

internal static class ExtensionMethods
{
    public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
    {
        foreach (var item in values) action(item);
    }

    public static IEnumerable<T> Select<T>(this Array array, Func<object?, T> projection)
    {
        foreach (var item in array) yield return projection(item);
    }

    /// <summary>
    /// Returns a new dictionary that contains all the elements of the original dictionary except the ones with the specified keys.
    /// </summary>
    /// <param name="value">A dictionary to exclude the specified keys.</param>
    /// <param name="keysToExclude">Key strings to exclude from the dictionary.</param>
    /// <returns>A dictionary that contains all the elements of the original dictionary except the ones with the specified keys.</returns>
    public static IReadOnlyDictionary<string, object?> Exclude(this IReadOnlyDictionary<string, object?> value, params string[] keysToExclude)
    {
        return keysToExclude.Length == 0 ? value : value
            .Where(item => !keysToExclude.Contains(item.Key))
            .ToDictionary(item => item.Key, item => item.Value);
    }

    /// <summary>
    /// Returns the type is a generic type of the specified type.
    /// </summary>
    /// <param name="targetType">A type object to compare with the specified generics type.</param>
    /// <param name="genericsType">A generics type to compare with the specified type.</param>
    /// <returns>Returns true if the specified type is the specified generics type. otherwise, false.</returns>
    public static bool IsGenericTypeOf(this Type targetType, Type genericsType)
    {
        return targetType.IsGenericType && targetType.GetGenericTypeDefinition() == genericsType;
    }
}
