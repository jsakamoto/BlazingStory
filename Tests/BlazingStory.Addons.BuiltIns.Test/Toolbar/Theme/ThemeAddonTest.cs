using System.Diagnostics.CodeAnalysis;
using BlazingStory.Addons.BuiltIns.Toolbar.Theme;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons.BuiltIns.Test.Toolbar.Theme;

public class ThemeAddonTest
{
    [Test]
    public void Initialize_RegistersThemeToolbarContent_ForStoryDocsAndCustomPage_Test()
    {
        // Given
        var builder = new TestAddonBuilder();

        // When
        new ThemeAddon().Initialize(builder);

        // Then
        builder.ToolbarContentType.Is(typeof(ThemeToolbarContent));
        builder.Match.IsNotNull();
        builder.Match(ViewMode.Story).IsTrue();
        builder.Match(ViewMode.Docs).IsTrue();
        builder.Match(ViewMode.CustomPage).IsTrue();
    }

    private sealed class TestAddonBuilder : IAddonBuilder
    {
        public Type? ToolbarContentType { get; private set; }

        public Func<ViewMode, bool>? Match { get; private set; }

        public void AddToolbarContent<[DynamicallyAccessedMembers(All)] TToolbarContentComponent>(int order, Func<ViewMode, bool> match)
        {
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
