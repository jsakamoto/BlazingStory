namespace MyBlazorWasmApp1.Models;

/// <summary>
/// Represents the minimal user contract exposed to Blazor components and services.
/// </summary>
public interface IUser
{
    /// <summary>
    /// Gets or sets the display name of the user.
    /// </summary>
    string? Name { get; set; }
}
