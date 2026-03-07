using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons;

public interface IAddonBuilder
{
    void AddToolbarContent<[DynamicallyAccessedMembers(PublicConstructors | PublicMethods | PublicFields | PublicProperties | PublicEvents | PublicNestedTypes)] TToolbarContentComponent>(int order, Func<ViewMode, bool> match);
    void AddPreviewDecorator<[DynamicallyAccessedMembers(PublicConstructors | PublicMethods | PublicFields | PublicProperties | PublicEvents | PublicNestedTypes)] TPreviewDecoratorComponent>();
}
