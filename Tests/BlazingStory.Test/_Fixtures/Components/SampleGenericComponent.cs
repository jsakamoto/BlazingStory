using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test._Fixtures.Components;

internal class SampleGenericComponent<TItem> : ComponentBase
{
    [Parameter]
    public RenderFragment<SampleGenericComponentContext<TItem>>? ItemTemplate { get; set; }

    internal class SampleGenericComponentContext<T>
    { }
}
