using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Pages.Canvas.Controls;

public partial class ControlsPanel : ComponentBase
{
    #region Public Properties

    [Parameter, EditorRequired]
    public Story? Story { get; set; }

    [Parameter]
    public bool ShowDetails { get; set; }

    #endregion Public Properties

    #region Private Fields

    private Story? _LastShownStory;

    #endregion Private Fields

    #region Protected Methods

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (this._LastShownStory != this.Story)
        {
            foreach (var param in this.GetParameters())
            {
                await param.UpdateSummaryFromXmlDocCommentAsync();
            }

            this._LastShownStory = this.Story;
        }
    }

    #endregion Protected Methods

    #region Private Methods

    /// <summary>
    /// Get parameters of the component from the current story.
    /// </summary>
    private IEnumerable<ComponentParameter> GetParameters()
    {
        return this.Story?.Context.Parameters ?? Enumerable.Empty<ComponentParameter>();
    }

    private string GetKey(ComponentParameter parameter)
    {
        return (this.Story?.NavigationPath ?? "") + ":" + parameter.Name;
    }

    private object? GetArgumentValue(string parameterName)
    {
        return this.Story?.Context.Args.TryGetValue(parameterName, out var value) == true ? value : default;
    }

    private async Task ResetControls()
    {
        if (this.Story == null)
        {
            return;
        }

        await this.Story.Context.ResetArgumentsAsync();
    }

    private async Task OnInputAsync(object? value, ComponentParameter parameter)
    {
        if (this.Story == null)
        {
            return;
        }

        await this.Story.Context.AddOrUpdateArgumentAsync(parameter.Name, value);
    }

    #endregion Private Methods
}
