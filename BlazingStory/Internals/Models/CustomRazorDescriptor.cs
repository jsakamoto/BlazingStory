using BlazingStory.Types;

namespace BlazingStory.Internals.Models;
internal class CustomRazorDescriptor
{
    internal readonly CustomAttribute CustomAttribute;

    public CustomRazorDescriptor(CustomAttribute customAttribute)
    {
        this.CustomAttribute = customAttribute;
    }
}
