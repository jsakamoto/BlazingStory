using System.Text.Json.Serialization;
using BlazingStory.Internals.Pages.TableOfContents;
using BlazingStory.Internals.Utils;
using BlazingStory.ToolKit.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Pages;

public class MarkdownPageBase : ComponentBase, IAsyncDisposable
{
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

    public MarkdownPageBase()
    {
        this.JSModule = JSModuleFactory.Create(() => this.JSRuntime, "js/markdown-page.js");
    }

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
