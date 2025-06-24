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
        public EventCallback EventCallback { get; init; } = default;
        public EventCallback<DateTime> EventCallbackWithArg { get; init; } = default;
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

    private class ClassWithIndexer
    {
        // This indexer should be omitted by the converter logic
        public int this[int key] => key + 42;

        // Normal property
        public string NormalProperty { get; set; } = string.Empty;
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
              "EventCallback": "Serialization of type 'EventCallback' is not supported.",
              "EventCallbackWithArg": "Serialization of type 'EventCallback<DateTime>' is not supported.",
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

    [Test]
    public void JsonFallbackSerializer_Serialize_ObjectWithIndexer_Test()
    {
        // GIVEN
        var withIndexer = new ClassWithIndexer
        {
            NormalProperty = "TestValue"
        };

        // WHEN
        var json = JsonFallbackSerializer.Serialize(withIndexer,
            options => options.WriteIndented = true);

        // THEN: ensure it includes NormalProperty but ignores the indexer
        // The indexer should not appear, since it's skipped by p.GetIndexParameters().Length == 0
        Assert.That(json, Does.Contain("NormalProperty"));
        Assert.That(json, Does.Not.Contain("42")); // or any marker for the indexer value
    }

    private class TestComponent : ComponentBase
    {
        public string? Name { get; set; }
        public EventCallback<ChangeEventArgs> OnChange { get; set; }
        public RenderFragment? ChildContent { get; set; }
    }

    private class ObjectTypeProperty
    {
        public object? Value { get; set; } = new TestComponent
        {
            Name = "Test",
            OnChange = EventCallback<ChangeEventArgs>.Empty,
            ChildContent = builder => { }
        };
        public object? SelfReference { get; set; }
        public List<object?> ObjectList { get; set; }
        public Dictionary<string, object?> ObjectDictionary { get; set; }
        public ObjectTypeProperty()
        {
            this.SelfReference = this;
            this.ObjectList = [null, this, this.Value];
            this.ObjectDictionary = new()
            {
                ["Null"] = null,
                ["Self"] = this,
                ["Component"] = this.Value
            };
        }
    }

    [Test]
    public void JsonFallbackSerializer_Serialize_ObjectTypeProperty_Test()
    {
        // GIVEN
        var args = new ObjectTypeProperty();

        // WHEN
        var json = JsonFallbackSerializer.Serialize(args, options => options.WriteIndented = true);

        // THEN
        json.Is("""
            {
              "Value": {
                "Name": "Test",
                "OnChange": "Serialization of type 'EventCallback<ChangeEventArgs>' is not supported.",
                "ChildContent": "Serialization of type 'RenderFragment' is not supported."
              },
              "SelfReference": "Serialization of cyclic references is not supported.",
              "ObjectList": [
                null,
                "Serialization of cyclic references is not supported.",
                {
                  "Name": "Test",
                  "OnChange": "Serialization of type 'EventCallback<ChangeEventArgs>' is not supported.",
                  "ChildContent": "Serialization of type 'RenderFragment' is not supported."
                }
              ],
              "ObjectDictionary": {
                "Null": null,
                "Self": "Serialization of cyclic references is not supported.",
                "Component": {
                  "Name": "Test",
                  "OnChange": "Serialization of type 'EventCallback<ChangeEventArgs>' is not supported.",
                  "ChildContent": "Serialization of type 'RenderFragment' is not supported."
                }
              }
            }
            """);
    }

    [Test]
    public void JsonFallbackSerializer_Serialize_DateTime_Test()
    {
        // GIVEN
        var args = DateTime.Parse("2023-10-01T14:03:01.023Z");

        // WHEN
        var json = JsonFallbackSerializer.Serialize(args);

        // THEN
        json.Is("2023-10-01T14:03:01.0230000Z");
    }

    [Test]
    public void JsonFallbackSerializer_Serialize_DateTimeOffset_Test()
    {
        // GIVEN
        var args = DateTimeOffset.Parse("2025-09-20T07:01:03.652+09:00");

        // WHEN
        var json = JsonFallbackSerializer.Serialize(args);

        // THEN
        json.Is("2025-09-19T22:01:03.6520000Z");
    }

    [Test]
    public void JsonFallbackSerializer_Serialize_DateOnly_Test()
    {
        // GIVEN
        var args = DateOnly.Parse("2024-01-20");

        // WHEN
        var json = JsonFallbackSerializer.Serialize(args);

        // THEN
        json.Is("2024-01-20");
    }

    [Test]
    public void JsonFallbackSerializer_Serialize_TimeOnly_Test()
    {
        // GIVEN
        var args = TimeOnly.Parse("15:40:01");

        // WHEN
        var json = JsonFallbackSerializer.Serialize(args);

        // THEN
        json.Is("15:40:01.0000000");
    }

    [Test]
    public void JsonFallbackSerializer_Serialize_Boolean_Test()
    {
        JsonFallbackSerializer.Serialize(true).Is("true");
        JsonFallbackSerializer.Serialize(false).Is("false");
    }
}
