using BlazingStory.Addons.BuiltIns.Panel.Actions;
using Microsoft.AspNetCore.Components.Web;

namespace BlazingStory.Addons.BuiltIns.Test.Panel.Actions;

public class ActionsPanelPreviewDecoratorTest
{
    [Test]
    public void CreateEventTArgMonitorHandlerDelegate_MethodHasOnlyEventArgsParameter_Test()
    {
        // Given
        var decorator = new ActionsPanelPreviewDecorator();

        // When
        var handler = decorator.CreateEventTArgMonitorHandlerDelegate(typeof(MouseEventArgs), "OnClick");

        // Then
        // Blazor resolves the event args type of a custom event (registered by "Blazor.registerCustomEventType")
        // from the parameters of the handler's method, and it rejects a method that has more than one parameter.
        handler.IsInstanceOf<Func<MouseEventArgs, Task>>();
        handler.Method.GetParameters().Select(p => p.ParameterType).Is(typeof(MouseEventArgs));
    }
}
