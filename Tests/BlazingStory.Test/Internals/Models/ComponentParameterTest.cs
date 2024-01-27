using BlazingStory.Test._Fixtures.Components;

namespace BlazingStory.Test.Internals.Models;

internal class ComponentParameterTest
{
    [Test]
    public void GetParameterTypeStrings_For_BasicTypes_Test()
    {
        var number1 = SampleComponent.CreateComponentParameter(nameof(SampleComponent.Number1));
        number1.GetParameterTypeStrings().Is("int");
    }

    [Test]
    public void GetParameterTypeStrings_For_EventCallback_Test()
    {
        var callback1 = SampleComponent.CreateComponentParameter(nameof(SampleComponent.Callback1));
        callback1.GetParameterTypeStrings().Is("EventCallback");

        var callback2 = SampleComponent.CreateComponentParameter(nameof(SampleComponent.Callback2));
        callback2.GetParameterTypeStrings().Is("EventCallback<string>");
    }

    [Test]
    public void GetParameterTypeStrings_For_Enum_Test()
    {
        var enum1 = SampleComponent.CreateComponentParameter(nameof(SampleComponent.Enum1));
        enum1.GetParameterTypeStrings().Is("SampleEnum", "\"Lorem\"", "\"Ipsum\"");

        var enum2 = SampleComponent.CreateComponentParameter(nameof(SampleComponent.Enum2));
        enum2.GetParameterTypeStrings().Is("SampleEnum?", "\"Lorem\"", "\"Ipsum\"");
    }
}
