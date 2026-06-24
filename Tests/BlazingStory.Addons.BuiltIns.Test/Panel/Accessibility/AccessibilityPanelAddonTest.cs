using System.Reflection;
using BlazingStory.Addons;
using BlazingStory.Addons.BuiltIns.Panel.Accessibility;
using BlazingStory.Addons.BuiltIns.Panel.Accessibility.Axe;

namespace BlazingStory.Addons.BuiltIns.Test.Panel.Accessibility;

public class AccessibilityPanelAddonTest
{
    [Test]
    public void Initialize_RegistersPanelAndPreviewDecorator()
    {
        var addon = new AccessibilityPanelAddon();
        var builder = new TestAddonBuilder();

        addon.Initialize(builder);

        builder.PanelType.Is(typeof(AccessibilityPanel));
        builder.PanelOrder.Is(300);
        builder.PanelMatch.IsNotNull();
        builder.PanelMatch!(ViewMode.Story).IsTrue();
        builder.PanelMatch!(ViewMode.Docs).IsFalse();
        builder.PreviewDecoratorType.Is(typeof(AccessibilityPanelPreviewDecorator));
    }

    [Test]
    public void Initialize_NullBuilder_Throws()
    {
        var addon = new AccessibilityPanelAddon();
        Action action = () => addon.Initialize(builder: null!);

        Assert.That(action, Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void NodeResult_Html_NullInput_IsHandled()
    {
        var node = new NodeResult { Html = null! };

        node.Html.Is("");
    }

    [Test]
    public async Task AccessibilityPanel_OnMessageReceived_NullMessage_DoesNotThrow()
    {
        var panel = new AccessibilityPanel();
        var onMessageReceived = typeof(AccessibilityPanel)
            .GetMethod("OnMessageReceived", BindingFlags.Instance | BindingFlags.NonPublic)
            .IsNotNull();

        var task = onMessageReceived.Invoke(panel, [null]).IsInstanceOf<Task>();

        await task;
    }

    [Test]
    public async Task AccessibilityPanel_OnReceiveAxeResultAsync_WhitespaceUrl_DoesNotThrow()
    {
        var panel = new AccessibilityPanel();
        var onReceiveAxeResultAsync = typeof(AccessibilityPanel)
            .GetMethod("OnReceiveAxeResultAsync", BindingFlags.Instance | BindingFlags.NonPublic)
            .IsNotNull();

        var task = onReceiveAxeResultAsync.Invoke(panel, ["   "]).IsInstanceOf<Task>();

        await task;
    }

    private sealed class TestAddonBuilder : IAddonBuilder
    {
        public Type? PanelType { get; private set; }

        public int PanelOrder { get; private set; }

        public Func<ViewMode, bool>? PanelMatch { get; private set; }

        public Type? PreviewDecoratorType { get; private set; }

        public void AddToolbarContent<TToolbarContentComponent>(int order, Func<ViewMode, bool> match)
        {
        }

        public void AddPanel<TPanelComponent>(int order, Func<ViewMode, bool> match)
        {
            this.PanelType = typeof(TPanelComponent);
            this.PanelOrder = order;
            this.PanelMatch = match;
        }

        public void AddPreviewDecorator<TPreviewDecoratorComponent>()
        {
            this.PreviewDecoratorType = typeof(TPreviewDecoratorComponent);
        }
    }
}
