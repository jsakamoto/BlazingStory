using MyBlazorWasmApp1.Models;

namespace MyBlazorWasmApp1.Stories.Models;

/// <summary>
/// Represents a user model used in stories for the Blazor WebAssembly application.
/// Implements <see cref="IUser"/>.
/// </summary>
public class User : IUser
{
    /// <summary>
    /// Gets or sets the display name of the user.
    /// </summary>
    public string? Name { get; set; }
}
