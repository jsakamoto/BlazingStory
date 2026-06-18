using BlazingStory.Abstractions;
using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test.Shared._Fixtures.Components;

public enum SampleEnum
{
    Lorem,
    Ipsum
}

public class SampleComponent
{
    [Parameter]
    public int Number1 { get; set; }

    public int Number2 { get; set; } // Not a parameter

    [Parameter]
    public EventCallback Callback1 { get; set; }

    [Parameter]
    public EventCallback<string> Callback2 { get; set; }

    [Parameter]
    public SampleEnum Enum1 { get; set; }

    [Parameter]
    public SampleEnum? Enum2 { get; set; }

    [Parameter]
    public int[]? Numbers { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public RenderFragment<SampleComponent>? Template1 { get; set; }

    public static IComponentParameter CreateComponentParameter(string propertyName)
    {
        return new ComponentParameter(
            typeof(SampleComponent),
            typeof(SampleComponent).GetProperty(propertyName)!,
            XmlDocComment.Dummy);
    }
}
