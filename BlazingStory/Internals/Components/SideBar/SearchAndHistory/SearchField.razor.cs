using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Command;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Components.SideBar.SearchAndHistory;

public partial class SearchField : ComponentBase
{
    #region Public Properties

    [Parameter]
    public string? SearchText { get; set; }

    [Parameter]
    public EventCallback<string?> SearchTextChanged { get; set; }

    [Parameter]
    public EventCallback Focus { get; set; }

    [Parameter]
    public EventCallback Cleared { get; set; }

    #endregion Public Properties

    #region Protected Properties

    [CascadingParameter]
    protected IServiceProvider Services { get; init; } = default!;

    #endregion Protected Properties

    #region Private Properties

    private bool _SearchBoxHasText => !string.IsNullOrEmpty(this.SearchText);
    private string SearchBoxPlaceHolder => this._SearchBoxHasFocus ? "Type to find..." : "Find components";

    #endregion Private Properties

    #region Private Fields

    private ElementReference _InputElement;

    private bool _SearchBoxHasFocus = false;
    private HotKeyCombo? _GoSearchHotKey;

    #endregion Private Fields

    #region Protected Methods

    protected override void OnInitialized()
    {
        var commandService = this.Services.GetRequiredService<CommandService>();
        var goSearchCommand = commandService[CommandType.GoSearch];

        if (goSearchCommand == null)
        {
            return;
        }

        this._GoSearchHotKey = goSearchCommand.HotKey;
        goSearchCommand.Subscribe(this.OnGoSearchCommand);
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task OnInputSearchText(ChangeEventArgs args)
    {
        this.SearchText = args.Value as string;
        await this.SearchTextChanged.InvokeAsync(this.SearchText);
    }

    private async Task OnFocusSearchBox()
    {
        await this.Focus.InvokeAsync();
        this._SearchBoxHasFocus = true;
    }

    private void OnBlurSearchBox() => this._SearchBoxHasFocus = false;

    private async ValueTask OnGoSearchCommand()
    {
        await Task.Delay(100);
        await this._InputElement.FocusAsync();
    }

    private async Task ClearSearchText()
    {
        this.SearchText = "";
        await this.SearchTextChanged.InvokeAsync(this.SearchText);
        await this.Cleared.InvokeAsync();
    }

    #endregion Private Methods
}
