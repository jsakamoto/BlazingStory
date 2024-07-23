using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Components.Markdown;

public partial class MarkdownDisplay : ComponentBase
{
    #region Private Fields

    private string RenderedHtml = string.Empty;

    #endregion Private Fields

    #region Public Properties

    [Parameter]
    public string? MarkdownString { get; set; }

    #endregion Public Properties

    #region Protected Methods

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (!string.IsNullOrWhiteSpace(this.MarkdownString))
        {
            this.RenderedHtml = Markdig.Markdown.ToHtml(this.MarkdownString);
        }
    }

    #endregion Protected Methods
}
