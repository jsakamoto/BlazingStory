using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;
using BlazingStory.Addons.BuiltIns.Toolbar.Theme;

namespace BlazingStory.Addons.BuiltIns.Test.Toolbar.Theme;

public class ThemeAddonTest
{
    [Test]
    public void Initialize_registers_theme_toolbar_content_with_ui_group_order_for_supported_views()
    {
        var builder = new TestAddonBuilder();

        new ThemeAddon().Initialize(builder);

        builder.ToolbarContentType.Is(typeof(ThemeToolbarContent));
        builder.ToolbarContentOrder.Is(50);
        builder.Match!(ViewMode.Story).IsTrue();
        builder.Match!(ViewMode.Docs).IsTrue();
        builder.Match!(ViewMode.CustomPage).IsTrue();
        builder.Match!((ViewMode)(-1)).IsFalse();
    }

    private sealed class TestAddonBuilder : IAddonBuilder
    {
        public int ToolbarContentOrder { get; private set; }

        public Type? ToolbarContentType { get; private set; }

        public Func<ViewMode, bool>? Match { get; private set; }

        public void AddToolbarContent<[DynamicallyAccessedMembers(All)] TToolbarContentComponent>(int order, Func<ViewMode, bool> match)
        {
            this.ToolbarContentOrder = order;
            this.ToolbarContentType = typeof(TToolbarContentComponent);
            this.Match = match;
        }

        public void AddPanel<[DynamicallyAccessedMembers(All)] TPanelComponent>(int order, Func<ViewMode, bool> match)
        {
        }

        public void AddPreviewDecorator<[DynamicallyAccessedMembers(All)] TPreviewDecoratorComponent>()
        {
        }
    }
}
