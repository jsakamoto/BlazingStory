using System.Diagnostics.CodeAnalysis;
using BlazingStory.Configurations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Components;

/// <summary>
/// A server-side Blazor component used to determine whether the current request is for the index
/// page or the iframe page.
///
/// - If the request matches the iframe page, renders <typeparamref name="TIFramePage" />.
/// - Otherwise, renders <typeparamref name="TIndexPage" />.
///
/// This component is sealed (cannot be inherited).
/// </summary>
/// <typeparam name="TIndexPage">
/// The type representing the index page component.
/// </typeparam>
/// <typeparam name="TIFramePage">
/// The type representing the iframe page component.
/// </typeparam>
public partial class BlazingStoryServerComponent<
    [DynamicallyAccessedMembers(All)] TIndexPage,
    [DynamicallyAccessedMembers(All)] TIFramePage> : ComponentBase
    where TIndexPage : ComponentBase
    where TIFramePage : ComponentBase
{
    // Provides access to the current navigation context (URI, base path, etc.)
    [Inject] private NavigationManager? NavigationManager { get; set; }

    // Provides configured subpath for Blazor hosting (e.g., "/blazor")
    [Inject] private BlazorSubpathConfig? SubpathConfig { get; set; }

    // Logger for diagnostics and debugging
    [Inject] private ILogger<BlazingStoryServerComponent<TIndexPage, TIFramePage>>? Logger { get; set; }

    // Flag indicating whether the current request is for iframe.html
    private bool _RequestForIFrameHtml = false;

    /// <summary>
    /// Initialization logic:
    /// - Ensures required services are injected
    /// - Parses current request URI
    /// - Compares it to the expected iframe.html path
    /// - Logs result for diagnostics
    /// </summary>
    protected override void OnInitialized()
    {
        if (this.NavigationManager == null)
        {
            throw new InvalidOperationException("NavigationManager is not injected.");
        }
        if (this.SubpathConfig == null)
        {
            throw new InvalidOperationException("BlazorSubpathConfig is not injected.");
        }

        if (!Uri.TryCreate(this.NavigationManager.Uri, UriKind.Absolute, out var uri))
        {
            return; // Invalid URI; skip processing
        }

        // Build the expected iframe path (e.g., /subpath/iframe.html)
        var iframeRelativePath = $"{this.SubpathConfig.Subpath}/iframe.html".Replace("//", "/");
        var iframePath = this.NavigationManager.ToAbsoluteUri(iframeRelativePath).AbsolutePath;

        // Mark request as iframe if paths match
        this._RequestForIFrameHtml = uri.AbsolutePath == iframePath;

        // Debug logging: current URI, expected iframe path, match flag
        this.Logger?.LogDebug("Current URI: {Uri}, Iframe Path: {IframePath}, IsIframe: {IsIframe}",
            uri.AbsolutePath, iframePath, this._RequestForIFrameHtml);
    }
}
