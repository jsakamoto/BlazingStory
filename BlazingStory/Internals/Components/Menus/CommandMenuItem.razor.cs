using BlazingStory.Internals.Services.Command;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.Menus;

public partial class CommandMenuItem : ComponentBase, IDisposable
{
    #region Public Properties

    [Parameter, EditorRequired]
    public Command? Command { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void Dispose()
    {
        if (this.Command == null)
        {
            return;
        }

        this.Command.StateChanged -= this.Command_StateChanged;
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void OnInitialized()
    {
        if (this.Command == null)
        {
            return;
        }

        this.Command.StateChanged += this.Command_StateChanged;
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task OnClickMenuItem()
    {
        if (this.Command == null)
        {
            return;
        }

        await this.Command.InvokeAsync();
    }

    private void Command_StateChanged(object? sender, EventArgs args)
    {
        this.StateHasChanged();
    }

    #endregion Private Methods
}
