using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Docs;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Pages.Docs;

public partial class CodeView : ComponentBase, IAsyncDisposable
{
    #region Public Properties

    [Parameter]
    public Story? Story { get; set; }

    [Parameter]
    public bool Visible { get; set; }

    #endregion Public Properties

    #region Internal Properties

    [CascadingParameter]
    internal IServiceProvider Services { get; init; } = default!;

    #endregion Internal Properties

    #region Private Properties

    [Inject] private IJSRuntime? JSRuntime { get; set; }

    #endregion Private Properties

    #region Private Fields

    private readonly JSModule _JSModule;
    private string? _SourceCodeText = null;

    private string? _CodeText;

    private bool _Copied = false;
    private ElementReference _CodeElementRef;

    private bool _PrevVisible = false;

    private int _RenderGen = 1;

    private int _HighlightedGen = 0;

    #endregion Private Fields

    #region Public Constructors

    public CodeView()
    {
        this._JSModule = new(() => this.JSRuntime, "prism.js");
    }

    #endregion Public Constructors

    #region Public Methods

    public async ValueTask DisposeAsync()
    {
        if (this.Story != null)
        {
            this.Story.Context.ArgumentChanged -= this.Context_ArgumentChanged;
        }

        await this._JSModule.DisposeAsync();
    }

    #endregion Public Methods

    #region Protected Methods

    protected override async Task OnParametersSetAsync()
    {
        base.OnParametersSet();

        if (!this._PrevVisible && this.Visible)
        {
            this._RenderGen++;
        }

        this._PrevVisible = this.Visible;

        if (this.Visible == true && this._SourceCodeText == null && this.Story != null)
        {
            this._SourceCodeText = await StoriesRazorSource.GetSourceCodeAsync(this.Story);
            this.UpdateCodeTextWithArgument();
        }
    }

    protected override void OnInitialized()
    {
        if (this.Story == null)
        {
            throw new InvalidOperationException("Story is required");
        }

        this.Story.Context.ArgumentChanged += this.Context_ArgumentChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.Visible && this._HighlightedGen != this._RenderGen)
        {
            await this._JSModule.InvokeVoidAsync("highlightElement", this._CodeElementRef);
            this._HighlightedGen = this._RenderGen;
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private ValueTask Context_ArgumentChanged()
    {
        this.UpdateCodeTextWithArgument();
        this._RenderGen++;
        this.StateHasChanged();

        return ValueTask.CompletedTask;
    }

    private void UpdateCodeTextWithArgument()
    {
        this._CodeText = this._SourceCodeText;

        if (this.Story == null || this._SourceCodeText == null)
        {
            return;
        }

        this._CodeText = StoriesRazorSource.UpdateSourceTextWithArgument(this.Story, this._SourceCodeText);
    }

    private async Task OnClickCopyButton()
    {
        if (this._CodeText == null)
        {
            return;
        }

        await this.Services.GetRequiredService<HelperScript>().CopyTextToClipboardAsync(this._CodeText);
        this._Copied = true;
        this.StateHasChanged();

        await Task.Delay(1500);
        this._Copied = false;
    }

    #endregion Private Methods
}
