namespace BlazingStory.Types;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class CustomPageAttribute(string title) : Attribute
{
    public string? Title { get; init; } = title;
}