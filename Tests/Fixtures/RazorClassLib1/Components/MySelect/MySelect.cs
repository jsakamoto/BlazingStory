using Microsoft.AspNetCore.Components;

namespace RazorClassLib1.Components.MySelect;

/// <summary>
/// A generic select component. See also <see cref="MySelect{TKey, TValue}.Value"/>.
/// </summary>
/// <typeparam name="TKey">A type representing the key of an item.</typeparam>
/// <typeparam name="TValue">A type representing the value of an item.</typeparam>
public class MySelect<TKey, TValue> : MySelectBase<TValue>
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

    /// <summary>
    /// Ignored if <see cref="MySelectBase{TItem}.Disabled"/> is <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// A <see cref="string"/> shown above the <see cref="MySelect{TKey, TValue}"/>.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Updated by <see cref="Select(TKey, bool)"/>, <see cref="Convert{TOut}(TOut)"/> and <see cref="explicit operator List{TValue}(MySelect{TKey, TValue})"/>.
    /// </summary>
    [Parameter]
    public TKey? SelectedKey { get; set; }

#pragma warning disable CS1574, CS1584, CS1658 // The compiler emits these unresolvable crefs with a "!:" prefix.
    /// <summary>
    /// Sorted by <see cref="NoSuchMember"/> or <see cref="Dictionary{string, int}"/>.
    /// </summary>
#pragma warning restore CS1574, CS1584, CS1658
    [Parameter]
    public string? SortBy { get; set; }

    public void Select(TKey key, bool append) { }

    public TOut Convert<TOut>(TOut value) => value;

    public static explicit operator List<TValue>(MySelect<TKey, TValue> _) => [];
}

public class MySelectBase<TItem> : ComponentBase
{
    [Parameter]
    public bool Disabled { get; set; }
}
