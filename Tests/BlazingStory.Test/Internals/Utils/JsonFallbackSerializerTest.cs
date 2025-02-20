using BlazingStory.Internals.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazingStory.Test.Internals.Utils;

public class JsonFallbackSerializerTest
{
    [Test]
    public void JsonFallbackSerializer_Serialize_SimpleObject_Test()
    {
        // GIVEN
        var eventArgs = new ProgressEventArgs
        {
            LengthComputable = true,
            Loaded = 50,
            Total = 100,
            Type = "progress"
        };

        // WHEN
        var json = JsonFallbackSerializer.Serialize(eventArgs, options => options.WriteIndented = true);

        // THEN
        json.Is("""
            {
              "LengthComputable": true,
              "Loaded": 50,
              "Total": 100,
              "Type": "progress"
            }
            """);
    }

    public class TestEvent : EventArgs
    {
        public RenderFragment Fragment { get; init; } = builder => { builder.AddMarkupContent(0, "<div>Test</div>"); };
        public RenderFragment<int> FragmentWithArg { get; init; } = (int n) => builder => { builder.AddMarkupContent(0, $"<div>{n}</div>"); };
        public Action SystemAction { get; init; } = () => { };
        public Action<int, string> SystemActionWithArg { get; init; } = (int n, string s) => { };
        public Func<bool> SystemFunc { get; init; } = () => true;
        public Func<int, string, bool> SystemFuncWithArg { get; init; } = (int n, string s) => false;
        public Circular CircularObject { get; init; } = new();
    }

    public class Circular
    {
        public Circular SelfReference { get; set; }

        public List<Circular?> SelfReferenceList { get; set; }

        public Dictionary<string, Circular?> SelfReferenceDictionary { get; set; }

        public Circular()
        {
            this.SelfReference = this;
            this.SelfReferenceList = [null, this, null];
            this.SelfReferenceDictionary = new() {
                { "Null", null },
                { "Self", this }
            };
        }
    }

    [Test]
    public void JsonFallbackSerializer_Serialize_ComplicatedObject_Test()
    {
        // GIVEN
        var eventArgs = new TestEvent();

        // WHEN
        var json = JsonFallbackSerializer.Serialize(eventArgs, options => options.WriteIndented = true);

        // THEN
        json.Is("""
            {
              "Fragment": "Serialization of type 'RenderFragment' is not supported.",
              "FragmentWithArg": "Serialization of type 'RenderFragment<int>' is not supported.",
              "SystemAction": "Serialization of type 'Action' is not supported.",
              "SystemActionWithArg": "Serialization of type 'Action<int, string>' is not supported.",
              "SystemFunc": "Serialization of type 'Func<bool>' is not supported.",
              "SystemFuncWithArg": "Serialization of type 'Func<int, string, bool>' is not supported.",
              "CircularObject": {
                "SelfReference": "Serialization of cyclic references is not supported.",
                "SelfReferenceList": [
                  null,
                  "Serialization of cyclic references is not supported.",
                  null
                ],
                "SelfReferenceDictionary": {
                  "Null": null,
                  "Self": "Serialization of cyclic references is not supported."
                }
              }
            }
            """);
    }
}
