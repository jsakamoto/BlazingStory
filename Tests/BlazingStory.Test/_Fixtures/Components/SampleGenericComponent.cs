using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test._Fixtures.Components;

internal class SampleGenericComponent<TItem> : ComponentBase
{
    internal class SampleGenericComponentContext<T> { }

    [Parameter]
    public RenderFragment<SampleGenericComponentContext<TItem>>? ItemTemplate { get; set; }
}
