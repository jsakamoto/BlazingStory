using System.Diagnostics.CodeAnalysis;
using BlazingStory.Types;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Models;
internal class CustomPageRazorDescriptor
{
    [DynamicallyAccessedMembers(All)]
    internal readonly Type TypeOfCustomPageRazor;

    internal readonly CustomPageAttribute CustomPageAttribute;

    public CustomPageRazorDescriptor([DynamicallyAccessedMembers(All)] Type typeOfCustomPageRazor, CustomPageAttribute customPageAttribute)
    {
        this.CustomPageAttribute = customPageAttribute;
        this.TypeOfCustomPageRazor = typeOfCustomPageRazor;
    }
}
