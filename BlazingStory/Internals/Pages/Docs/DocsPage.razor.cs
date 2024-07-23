using System.Text.RegularExpressions;
using BlazingStory.Components;
using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Addons;
using BlazingStory.Internals.Services.Command;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Internals.Pages.Docs;

public partial class DocsPage : ComponentBase, IDisposable
{
    #region Protected Properties

    [CascadingParameter]
    protected AddonsStore AddonsStore { get; init; } = default!;

    [CascadingParameter]
    protected StoriesStore StoriesStore { get; init; } = default!;

    [CascadingParameter]
    protected QueryRouteData RouteData { get; init; } = default!;

    [CascadingParameter]
    protected IServiceProvider Services { get; init; } = default!;

    [CascadingParameter]
    protected BlazingStoryApp BlazingStoryApp { get; init; } = default!;

    #endregion Protected Properties

    #region Private Fields

    private readonly Subscriptions _Subscriptions = new();
    private string? _CurrentNavigationPath;

    private string _PageTitle = "";

    private StoryContainer? _StoryComponent;

    private (Command? ToolbarVisible, Command? ShowSidebar, Command? Fullscreen) _Commands = default;

    #endregion Private Fields

    #region Public Methods

    public void Dispose()
    {
        this.AddonsStore.OnFrameArgumentsChanged -= this.AddonsStore_OnFrameArgumentsChanged;
        this._Subscriptions.Dispose();

        if (this._StoryComponent != null)
        {
            foreach (var story in this._StoryComponent.Stories)
            {
                story.Context.ArgumentChanged -= this.Context_ArgumentChanged;
            }
        }
    }

    #endregion Public Methods

    #region Protected Methods

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (this._CurrentNavigationPath != this.RouteData.Parameter)
        {
            if (this._StoryComponent != null)
            {
                foreach (var story in this._StoryComponent.Stories)
                {
                    story.Context.ArgumentChanged -= this.Context_ArgumentChanged;
                }

                this._StoryComponent = null;
            }

            this._CurrentNavigationPath = this.RouteData.Parameter;
            var navigationPath = Regex.Replace(this.RouteData.Parameter, "--docs$", "");

            if (!this.StoriesStore.TryGetComponentByPath(navigationPath, out var component))
            {
                return;
            }

            this._StoryComponent = component;
            this._PageTitle = string.Join(" / ", component.Title.Split('/')) + " - Docs - " + this.BlazingStoryApp.Title;

            foreach (var story in this._StoryComponent.Stories)
            {
                story.Context.ArgumentChanged += this.Context_ArgumentChanged;
            }

            await this._StoryComponent.UpdateSummaryFromXmlDocCommentAsync();
        }
    }

    protected override void OnInitialized()
    {
        this.AddonsStore.OnFrameArgumentsChanged += AddonsStore_OnFrameArgumentsChanged;

        var commands = this.Services.GetRequiredService<CommandService>();
        this._Commands = (
            ToolbarVisible: commands[CommandType.ToolBarVisible],
            ShowSidebar: commands[CommandType.SideBarVisible],
            Fullscreen: commands[CommandType.GoFullScreen]);
        this._Subscriptions.Add(this._Commands.ToolbarVisible!.Subscribe(this.OnToggleToolbarVisible));
    }

    #endregion Protected Methods

    #region Private Methods

    private ValueTask Context_ArgumentChanged()
    {
        this.StateHasChanged();
        return ValueTask.CompletedTask;
    }

    private void AddonsStore_OnFrameArgumentsChanged(object? sender, EventArgs args)
    {
        this.StateHasChanged();
    }

    private async ValueTask OnToggleToolbarVisible()
    {
        this._Commands.ToolbarVisible?.ToggleFlag();
        await ValueTask.CompletedTask;
    }

    #endregion Private Methods
}
