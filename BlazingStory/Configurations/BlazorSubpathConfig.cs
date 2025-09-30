namespace BlazingStory.Configurations;

/// <summary>
/// Holds configuration for the Blazor hosting subpath. This is typically injected as a singleton
/// and used by components or middleware to resolve the base path under which Blazor is served.
///
/// Example:
/// - "/" (root, default)
/// - "/blazor" (mounted under /blazor)
/// </summary>
public class BlazorSubpathConfig
{
    /// <summary>
    /// The base subpath where the Blazor app is hosted. Defaults to "/" (application root).
    /// </summary>
    public string Subpath { get; set; } = "/";
}
