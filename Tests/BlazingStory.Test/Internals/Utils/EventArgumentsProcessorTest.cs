using System.Text.Json;
using BlazingStory.Internals.Utils;
using BlazingStory.Test._Fixtures;
using BlazingStory.Test._Fixtures.Components;

namespace BlazingStory.Test.Internals.Utils;

internal class EventArgumentsProcessorTest
{
    [Test]
    public void MapEventNameToParameterName_Removes_Changed_Suffix_Test()
    {
        EventArgumentsProcessor.MapEventNameToParameterName("ValueChanged").Is("Value");
        EventArgumentsProcessor.MapEventNameToParameterName("CustomEvent").Is("CustomEvent");
    }

    [Test]
    public void ValidateTwoWayBindingPattern_Recognizes_Valid_Binding_Test()
    {
        var story = TestHelper.CreateStory<TwoWayBindingComponent>();

        EventArgumentsProcessor.ValidateTwoWayBindingPattern("ValueChanged", story, "Test").IsTrue();
        EventArgumentsProcessor.ValidateTwoWayBindingPattern("CountChanged", story, "Test").IsTrue();
    }

    [Test]
    public void ValidateTwoWayBindingPattern_Rejected_Invalid_Binding_Test()
    {
        var story = TestHelper.CreateStory<TwoWayBindingComponent>();

        EventArgumentsProcessor.ValidateTwoWayBindingPattern("Value", story, "Test").IsFalse();
        EventArgumentsProcessor.ValidateTwoWayBindingPattern("MissingChanged", story, "Test").IsFalse();
    }

    [Test]
    public void ExtractParameterValue_Reads_Direct_And_KeyValue_Event_Data_Test()
    {
        var directArgs = new Dictionary<string, object?>
        {
            ["Value"] = "hello"
        };

        var keyValueArgs = new Dictionary<string, object?>
        {
            ["Keys"] = JsonDocument.Parse("[\"Value\"]").RootElement,
            ["Values"] = JsonDocument.Parse("[\"world\"]").RootElement,
        };

        EventArgumentsProcessor.ExtractParameterValue(directArgs, "Value", typeof(string)).Is("hello");
        EventArgumentsProcessor.ExtractParameterValue(keyValueArgs, "Value", typeof(string)).Is("world");
    }
}