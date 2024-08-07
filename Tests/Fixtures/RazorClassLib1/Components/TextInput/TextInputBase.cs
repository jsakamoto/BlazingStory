using Microsoft.AspNetCore.Components;

namespace RazorClassLib1.Components.TextInput;

/// <summary>
/// A base class for text input components.
/// </summary>
/// <typeparam name="T">A type representing the value of the input.</typeparam>
public class TextInputBase<T> : ComponentBase
{
    /// <summary>
    /// Gets or sets the value of the input.
    /// </summary>
    [Parameter]
    public T? Value { get; set; }

    /// <summary>
    /// Gets or sets the callback that will be invoked when the value changes.
    /// </summary>
    [Parameter]
    public EventCallback<T?> ValueChanged { get; set; }
}
