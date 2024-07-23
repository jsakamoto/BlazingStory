using BlazingStory.Configurations;
using BlazingStory.Internals.Extensions;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using BlazingStory.Internals.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Components.Preview;

public partial class PreviewFrame : IAsyncDisposable
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the story.
    /// </summary>
    /// <value>
    /// The story.
    /// </value>
    [Parameter, EditorRequired]
    public Story? Story { get; set; }

    /// <summary>
    /// Gets or sets the view mode.
    /// </summary>
    /// <value>
    /// The view mode.
    /// </value>
    [Parameter]
    public string? ViewMode { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the arguments.
    /// </summary>
    /// <value>
    /// The arguments.
    /// </value>
    [Parameter]
    public IReadOnlyDictionary<string, object?>? Args { get; set; }

    /// <summary>
    /// Gets or sets the globals.
    /// </summary>
    /// <value>
    /// The globals.
    /// </value>
    [Parameter]
    public IReadOnlyDictionary<string, object?>? Globals { get; set; }

    /// <summary>
    /// Gets or sets the on component action.
    /// </summary>
    /// <value>
    /// The on component action.
    /// </value>
    [Parameter]
    public EventCallback<ComponentActionEventArgs> OnComponentAction { get; set; }

    /// <summary>
    /// Gets the initial preview frame URL.
    /// </summary>
    public string CurrentPreviewFrameUrl => this._CurrentPreviewFrameUrl;

    #endregion Public Properties

    #region Internal Properties

    [CascadingParameter]
    internal IServiceProvider Services { get; init; } = default!;

    #endregion Internal Properties

    #region Private Properties

    [Inject] private IJSRuntime? JSRuntime { get; set; }

    #endregion Private Properties

    #region Private Fields

    private readonly string _IframeElementId = "F" + Guid.NewGuid().ToString();
    private readonly JSModule _JSModule;
    private ElementReference _Iframe;

    private string _InitialPreviewFrameUrl = "";

    private string _CurrentPreviewFrameUrl = "";
    private bool _AfterFirstRendered = false;

    private DotNetObjectReference<PreviewFrame>? _ThisRef;

    private IJSObjectReference? _EventMonitorSubscriber;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PreviewFrame" /> class.
    /// </summary>
    public PreviewFrame()
    {
        this._JSModule = new(() => this.JSRuntime, "Internals/Components/Preview/PreviewFrame.razor.js");
    }

    #endregion Public Constructors

    #region Public Methods

    [JSInvokable(nameof(ComponentActionEventCallback))]
    public async Task ComponentActionEventCallback(string name, string argsJson)
    {
        await this.OnComponentAction.InvokeAsync(new(name, argsJson));
    }

    public async ValueTask DisposeAsync()
    {
        if (this._EventMonitorSubscriber is not null)
        {
            await this._EventMonitorSubscriber.DisposeIfConnectedAsync("dispose");
        }

        if (this._JSModule is not null)
        {
            await this._JSModule.DisposeAsync();
        }

        this._ThisRef?.Dispose();
    }

    #endregion Public Methods

    #region Internal Methods

    internal async ValueTask ReloadAsync()
    {
        if (this._JSModule is not null)
        {
            await this._JSModule.InvokeVoidAsync("reloadPreviewFrame", this._Iframe);
        }
    }

    internal async ValueTask ZoomInAsync()
    {
        if (this._JSModule is not null)
        {
            await this._JSModule.InvokeVoidAsync("zoomInPreviewFrame", this._Iframe);
        }
    }

    internal async ValueTask ZoomOutAsync()
    {
        if (this._JSModule is not null)
        {
            await this._JSModule.InvokeVoidAsync("zoomOutPreviewFrame", this._Iframe);
        }
    }

    internal async ValueTask ResetZoomAsync()
    {
        if (this._JSModule is not null)
        {
            await this._JSModule.InvokeVoidAsync("resetZoomPreviewFrame", this._Iframe);
        }
    }

    #endregion Internal Methods

    #region Protected Methods

    protected override void OnInitialized()
    {
        this._InitialPreviewFrameUrl = this._CurrentPreviewFrameUrl = this.GetPreviewFrameUrl();
    }

    protected override async Task OnParametersSetAsync()
    {
        await this.UpdatePreviewFrameUrlAsync();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        this.StateHasChanged();
        this._AfterFirstRendered = true;
    }

    #endregion Protected Methods

    #region Private Methods

    private async ValueTask UpdatePreviewFrameUrlAsync()
    {
        if (!this._AfterFirstRendered)
        {
            return;
        }

        var nextPreviewFrameUrl = this.GetPreviewFrameUrl();

        if (this._CurrentPreviewFrameUrl == nextPreviewFrameUrl)
        {
            return;
        }

        this._CurrentPreviewFrameUrl = nextPreviewFrameUrl;

        if (this._JSModule is not null)
        {
            await this._JSModule.InvokeVoidAsync("navigatePreviewFrameTo", this._Iframe, nextPreviewFrameUrl);
        }
    }

    private async Task OnPreviewIFrameLoaded(ProgressEventArgs obj)
    {
        try
        {
            var options = this.Services.GetRequiredService<BlazingStoryOptions>();

            if (options.EnableHotReloading)
            {
                await this.EnsureDotnetWatchScriptInjected();
            }

            this._ThisRef = DotNetObjectReference.Create(this);

            if (this._JSModule is not null)
            {
                this._EventMonitorSubscriber = await this._JSModule.InvokeAsync<IJSObjectReference>("subscribeComponentActionEvent", this._Iframe, this._ThisRef, nameof(ComponentActionEventCallback));
            }
        }

        // In some cases, such as when a user navigates away from the page during this async
        // operation, the JSModule may be disposed before the async operation completes. Ignore the
        // "ObjectDisposedException" exception in this case.
        catch (ObjectDisposedException)
        {
        }
    }

    /// <summary>
    /// Gets the URL for the preview frame, from the component parameters(args).
    /// </summary>
    private string GetPreviewFrameUrl()
    {
        if (this.Story is null)
        {
            return "./iframe.html";
        }

        // "Stringify" the component args to encode them to the URL query string.
        var args = this.Args?.ToDictionary(
            keySelector: x => x.Key,
            elementSelector: x => (object?)this.Story.Context.ConvertParameterValueToString(x.Key, x.Value)
        );

        var encodeArgs = UriParameterKit.EncodeKeyValues(args);
        var encodeGlobals = UriParameterKit.EncodeKeyValues(this.Globals);

        var compressedArgs = UrlParameterShortener.CompressAndEncode(encodeArgs);

        return UriParameterKit.GetUri(
            uri: "./iframe.html",
            parameters: new Dictionary<string, object?>
            {
                ["viewMode"] = this.ViewMode,
                ["id"] = this.Id,
                ["args"] = compressedArgs,
                ["globals"] = encodeGlobals
            }
        );
    }

    private async ValueTask EnsureDotnetWatchScriptInjected()
    {
        if (this._JSModule is not null)
        {
            await this._JSModule.InvokeVoidAsync("ensureDotnetWatchScriptInjected", this._Iframe);
        }
    }

    #endregion Private Methods
}
