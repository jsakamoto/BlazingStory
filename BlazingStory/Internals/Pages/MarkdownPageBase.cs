using System.Text.Json.Serialization;
using BlazingStory.Internals.Pages.TableOfContents;
using BlazingStory.Internals.Utils;
using BlazingStory.ToolKit.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Pages;

/// <summary>
/// Base component for markdown-based custom pages with TOC integration.
/// </summary>
public class MarkdownPageBase : ComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the JavaScript runtime instance.
    /// </summary>
    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the table-of-contents state supplied by the parent page.
    /// </summary>
    [CascadingParameter]
    internal CustomPageTableOfContentsState TableOfContentsState { get; set; } = new();

    private JSModule JSModule;

    /// <summary>
    /// Gets collected heading source entries from rendered markdown content.
    /// </summary>
    internal IReadOnlyList<TableOfContentsSourceHeading> TableOfContentsSourceHeadings { get; private set; } = [];

    /// <summary>
    /// Gets built table-of-contents items for rendered markdown content.
    /// </summary>
    internal IReadOnlyList<TableOfContentsItem> TableOfContentsItems { get; private set; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownPageBase"/> class.
    /// </summary>
    public MarkdownPageBase()
    {
        this.JSModule = JSModuleFactory.Create(() => this.JSRuntime, "js/markdown-page.js");
    }

    /// <summary>
    /// Performs post-render work such as syntax highlighting and TOC extraction.
    /// </summary>
    /// <param name="firstRender">Indicates whether this is the first render.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await this.JSModule.InvokeVoidAsync("formatCodeBlock", ".custom-page-contents pre:has(code)");

        var headings = await this.JSModule.InvokeAsync<JSImportHeading[]>("collectHeadings", ".custom-page-contents");
        this.TableOfContentsSourceHeadings = (headings ?? [])
            .Select(heading => new TableOfContentsSourceHeading(
                Text: heading.Text,
                Level: heading.Level,
                Id: heading.Id))
            .ToArray();

        this.TableOfContentsItems = TableOfContentsModelFactory.Create(
            this.TableOfContentsSourceHeadings,
            this.TableOfContentsState.MinHeadingLevel,
            this.TableOfContentsState.MaxHeadingLevel);
        this.TableOfContentsState.SetItems(this.TableOfContentsItems);
    }

    /// <summary>
    /// Asynchronously releases the JS module resources.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the dispose operation.</returns>
    public ValueTask DisposeAsync() => this.JSModule.DisposeAsync();

    /// <summary>
    /// Represents heading data collected from JavaScript.
    /// </summary>
    private sealed class JSImportHeading
    {
        /// <summary>
        /// Gets or sets the heading id.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        /// <summary>
        /// Gets or sets the heading text.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; } = "";

        /// <summary>
        /// Gets or sets the heading level.
        /// </summary>
        [JsonPropertyName("level")]
        public int Level { get; set; }
    }
}
