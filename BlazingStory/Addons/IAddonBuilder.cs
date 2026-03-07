using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Addons;

public interface IAddonBuilder
{
    void AddToolbarButton<[DynamicallyAccessedMembers(PublicConstructors | PublicMethods | PublicFields | PublicProperties | PublicEvents | PublicNestedTypes)] TToolbarButtonComponent>(int order, Func<ViewMode, bool> match);
    void AddPreviewDecorator<[DynamicallyAccessedMembers(PublicConstructors | PublicMethods | PublicFields | PublicProperties | PublicEvents | PublicNestedTypes)] TPreviewDecoratorComponent>();
}
