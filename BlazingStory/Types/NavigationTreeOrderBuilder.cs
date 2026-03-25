namespace BlazingStory.Types;

/// <summary>
/// Provides a lightweight builder to declare the order of navigation tree items.
/// </summary>
/// <remarks>
/// Typical usage is via the shared <see cref="N"/> instance and the indexer to produce an ordered
/// array of <see cref="NavigationTreeOrderEntry"/> instances.
/// </remarks>
public class NavigationTreeOrderBuilder
{
    /// <summary>
    /// Shared instance used as the entry point for building navigation orders.
    /// </summary>
    public static readonly NavigationTreeOrderBuilder N = new();

    /// <summary>
    /// Collects the specified <see cref="NavigationTreeOrderEntry"/> items and returns them as an array,
    /// preserving the provided order.
    /// </summary>
    /// <param name="items">The sequence describing the desired navigation order.</param>
    /// <returns>An array containing the supplied <paramref name="items"/> in the same order.</returns>
    public NavigationTreeOrderEntry[] this[params NavigationTreeOrderEntry[] items] => items;
}
