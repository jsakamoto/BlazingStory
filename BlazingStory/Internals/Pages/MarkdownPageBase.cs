using BlazingStory.Internals.Utils;
using BlazingStory.ToolKit.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Pages;

public class MarkdownPageBase : ComponentBase, IAsyncDisposable
{
    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    private JSModule JSModule;

    public MarkdownPageBase()
    {
        this.JSModule = JSModuleFactory.Create(() => this.JSRuntime, "js/markdown-page.js");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await this.JSModule.InvokeVoidAsync("formatCodeBlock", ".custom-page-contents pre:has(code)");
    }

    public ValueTask DisposeAsync() => this.JSModule.DisposeAsync();
}
