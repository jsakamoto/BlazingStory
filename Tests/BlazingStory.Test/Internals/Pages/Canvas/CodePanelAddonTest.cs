using System.Diagnostics.CodeAnalysis;
using BlazingStory.Addons;
using BlazingStory.Internals.Pages.Canvas;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Test.Internals.Pages.Canvas;

internal class CodePanelAddonTest
{
    [Test]
    public void Initialize_RegistersCodePanel_ForStoryViewOnly_Test()
    {
        // Given
        var builder = new TestAddonBuilder();

        // When
        new CodePanelAddon().Initialize(builder);

        // Then
        builder.PanelType.Is(typeof(CodePanel));
        builder.Match.IsNotNull();
        builder.Match(ViewMode.Story).IsTrue();
        builder.Match(ViewMode.Docs).IsFalse();
        builder.Match(ViewMode.CustomPage).IsFalse();
    }

    private sealed class TestAddonBuilder : IAddonBuilder
    {
        public Type? PanelType { get; private set; }

        public Func<ViewMode, bool>? Match { get; private set; }

        public void AddToolbarContent<[DynamicallyAccessedMembers(All)] TToolbarContentComponent>(int order, Func<ViewMode, bool> match)
        {
        }

        public void AddPanel<[DynamicallyAccessedMembers(All)] TPanelComponent>(int order, Func<ViewMode, bool> match)
        {
            this.PanelType = typeof(TPanelComponent);
            this.Match = match;
        }

        public void AddPreviewDecorator<[DynamicallyAccessedMembers(All)] TPreviewDecoratorComponent>()
        {
        }
    }
}
