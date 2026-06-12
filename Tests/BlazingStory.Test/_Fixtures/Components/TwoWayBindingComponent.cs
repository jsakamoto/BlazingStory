using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test._Fixtures.Components;

internal class TwoWayBindingComponent
{
    [Parameter]
    public string Value { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public int Count { get; set; }

    [Parameter]
    public EventCallback<int> CountChanged { get; set; }
}