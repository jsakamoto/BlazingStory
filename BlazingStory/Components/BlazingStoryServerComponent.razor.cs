using System.Diagnostics.CodeAnalysis;
using BlazingStory.Configurations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Components;

/// <summary>
/// A server-side Blazor component used to determine whether the current request is for the index
/// page or the iframe page.
///
/// - If the request matches the iframe page, renders <typeparamref name="TIFramePage" />.
/// - Otherwise, renders <typeparamref name="TIndexPage" />.
///
/// This version supports both:
/// - With DI registration of <see cref="BlazorSubpathConfig" />.
/// - Without DI registration (fallbacks automatically to "/").
/// </summary>
/// <typeparam name="TIndexPage">
/// The type representing the index page component.
/// </typeparam>
/// <typeparam name="TIFramePage">
/// The type representing the iframe page component.
/// </typeparam>
public partial class BlazingStoryServerComponent<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TIndexPage,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TIFramePage> : ComponentBase
    where TIndexPage : ComponentBase
    where TIFramePage : ComponentBase
{
    // Provides access to the current navigation context (URI, base path, etc.)
    [Inject] private NavigationManager? NavigationManager { get; set; }

    // Provides access to the global service provider (used to resolve optional services)
    [Inject] private IServiceProvider? Services { get; set; }

    // Holds the resolved or default subpath configuration
    private BlazorSubpathConfig _SubpathConfig = new();

    // Flag indicating whether the current request is for iframe.html
    private bool _RequestForIFrameHtml = false;

    /// <summary>
    /// Initialization logic:
    /// - Ensures required services are injected
    /// - Tries to resolve <see cref="BlazorSubpathConfig" /> from DI
    /// - Falls back to default configuration if not found
    /// - Determines if the current request is targeting iframe.html
    /// </summary>
    protected override void OnInitialized()
    {
        // Ensure NavigationManager is available (required in all Blazor apps)
        if (this.NavigationManager == null)
        {
            throw new InvalidOperationException("NavigationManager is not injected.");
        }

        // Try resolving BlazorSubpathConfig from DI container
        if (this.Services != null)
        {
            var resolved = this.Services.GetService<BlazorSubpathConfig>();
            if (resolved != null)
            {
                this._SubpathConfig = resolved;
            }
        }

        // If not registered, fallback to default configuration
        this._SubpathConfig ??= new BlazorSubpathConfig { Subpath = "/" };

        // Parse current URI
        if (!Uri.TryCreate(this.NavigationManager.Uri, UriKind.Absolute, out var uri))
        {
            return; // Invalid URI; skip processing
        }

        // Normalize subpath to avoid double slashes
        var normalizedSubpath = this._SubpathConfig.Subpath?.TrimEnd('/') ?? string.Empty;

        // Build the expected iframe path (e.g., /subpath/iframe.html)
        var iframeRelativePath = $"{normalizedSubpath}/iframe.html".Replace("//", "/");
        var iframePath = this.NavigationManager.ToAbsoluteUri(iframeRelativePath).AbsolutePath;

        // Mark request as iframe if paths match
        this._RequestForIFrameHtml = uri.AbsolutePath.Equals(iframePath, StringComparison.OrdinalIgnoreCase);
    }
}
