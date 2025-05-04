using BlazingStory.Internals.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazingStory.Internals.Pages;

public class MarkdownPageBase : ComponentBase
{
    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await using var prism = await this.JSRuntime.ImportAsync("prism.js");
        await prism.InvokeVoidAsync("Prism.highlightAll");
    }
}
