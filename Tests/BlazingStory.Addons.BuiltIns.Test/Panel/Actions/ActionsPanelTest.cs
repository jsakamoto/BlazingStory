using BlazingStory.Addons.BuiltIns.Panel.Actions;
using BlazingStory.Addons;
using BlazingStory.ToolKit.Styles;
using Bunit;

namespace BlazingStory.Addons.BuiltIns.Test.Panel.Actions;

public class ActionsPanelTest
{
    [Test]
    public async Task ActionsPanel_shows_logged_action_after_component_action_event_Test()
    {
        // Given
        using var ctx = new BunitContext();
        ctx.ComponentFactories.AddStub<PanelTitle>();
        ctx.ComponentFactories.AddStub<ImportStyleSheet>();

        var actionLogs = new List<ComponentActionLog>();
        var cut = ctx.Render<TestableActionsPanel>(builder => builder
            .Add(c => c.ActionLogs, actionLogs));

        // When
        await cut.InvokeAsync(async () => await cut.Instance.OnComponentAction(new ComponentActionEventArgs
        {
            Name = "manual-save",
            ArgsJson = "{\"ok\":true}"
        }));

        // Then
        cut.Markup.Contains("manual-save").IsTrue();
        actionLogs.Count.Is(1);
        actionLogs[0].Name.Is("manual-save");
    }

    private sealed class TestableActionsPanel : ActionsPanel
    {
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return Task.CompletedTask;
        }
    }
}
