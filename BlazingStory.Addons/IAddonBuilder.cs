using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons;

public interface IAddonBuilder
{
    void AddToolbarContent<[DynamicallyAccessedMembers(All)] TToolbarContentComponent>(int order, Func<ViewMode, bool> match);
    void AddPanel<[DynamicallyAccessedMembers(All)] TPanelComponent>(int order, Func<ViewMode, bool> match);
    void AddPreviewDecorator<[DynamicallyAccessedMembers(All)] TPreviewDecoratorComponent>();
}
