using System.Diagnostics.CodeAnalysis;
using BlazingStory.Types;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Models;
internal class CustomRazorDescriptor
{
    [DynamicallyAccessedMembers(All)]
    internal readonly Type TypeOfCustomRazor;

    internal readonly CustomAttribute CustomAttribute;

    public CustomRazorDescriptor([DynamicallyAccessedMembers(All)] Type typeOfCustomRazor, CustomAttribute customAttribute)
    {
        this.CustomAttribute = customAttribute;
        this.TypeOfCustomRazor = typeOfCustomRazor;
    }
}
