using BlazingStory.Internals.Models;
using BlazingStory.Internals.Services.XmlDocComment;
using Microsoft.AspNetCore.Components;
using Moq;

namespace BlazingStory.Test.Internals.Models;

internal class ComponentParameterTest
{
    private enum SampleEnum
    {
        Lorem,
        Ipsum
    }
    private class SampleClass
    {
        public int Number1 { get; set; }
        public EventCallback Calback1 { get; set; }
        public EventCallback<string> Calback2 { get; set; }
        public SampleEnum Enum1 { get; set; }
        public SampleEnum? Enum2 { get; set; }

        public static ComponentParameter CreateComponentParameter(string propertyName)
        {
            var xmlDocCommentMock = Mock.Of<IXmlDocComment>();
            return new ComponentParameter(
                typeof(SampleClass),
                typeof(SampleClass).GetProperty(propertyName)!,
                xmlDocCommentMock);
        }
    }

    [Test]
    public void GetParameterStrings_For_BasicTypes_Test()
    {
        var number1 = SampleClass.CreateComponentParameter(nameof(SampleClass.Number1));
        number1.GetParameterStrings().Is("int");
    }

    [Test]
    public void GetParameterStrings_For_EventCallback_Test()
    {
        var callback1 = SampleClass.CreateComponentParameter(nameof(SampleClass.Calback1));
        callback1.GetParameterStrings().Is("EventCallback");

        var callback2 = SampleClass.CreateComponentParameter(nameof(SampleClass.Calback2));
        callback2.GetParameterStrings().Is("EventCallback<string>");
    }

    [Test]
    public void GetParameterStrings_For_Enum_Test()
    {
        var enum1 = SampleClass.CreateComponentParameter(nameof(SampleClass.Enum1));
        enum1.GetParameterStrings().Is("SampleEnum", "\"Lorem\"", "\"Ipsum\"");

        var enum2 = SampleClass.CreateComponentParameter(nameof(SampleClass.Enum2));
        enum2.GetParameterStrings().Is("SampleEnum?", "\"Lorem\"", "\"Ipsum\"");
    }
}
