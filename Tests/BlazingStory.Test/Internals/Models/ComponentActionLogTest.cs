using System.Text.Json;
using BlazingStory.Internals.Models;

namespace BlazingStory.Test.Internals.Models;

public class ComponentActionLogTest
{
    [Test]
    public void Ctor_with_void_Test()
    {
        var actionLog = new ComponentActionLog("foo", "void");
        actionLog.Name.Is("foo");
        actionLog.ArgsJson.Is("void");
        actionLog.ArgsJsonElement.ValueKind.Is(JsonValueKind.Undefined);
    }
}
