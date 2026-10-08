using Microsoft.AspNetCore.Components;

namespace RazorClassLib1.Components.MySelect;

/// <summary>
/// A generic select component. See also <see cref="MySelect{TKey, TValue}.Value"/>.
/// </summary>
/// <typeparam name="TKey">A type representing the key of an item.</typeparam>
/// <typeparam name="TValue">A type representing the value of an item.</typeparam>
public class MySelect<TKey, TValue> : ComponentBase
{
    /// <summary>
    /// The selected value. If <see cref="Multiselect"/> is <see langword="true"/>, this property will be ignored.
    /// </summary>
    [Parameter]
    public TValue? Value { get; set; }

    /// <summary>
    /// Allows selecting more than one item. The default is <see langword="false"/>.
    /// </summary>
    [Parameter]
    public bool Multiselect { get; set; }

    /// <summary>
    /// <para>The placeholder text. Set <see langword="null"/> to hide it.</para>
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// The items to select from, keyed by <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>.
    /// </summary>
    [Parameter]
    public IReadOnlyDictionary<TKey, TValue>? Items { get; set; }
}
