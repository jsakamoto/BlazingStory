namespace BlazingStory.Types;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class CustomAttribute(string title) : Attribute
{
    public string? Title { get; init; } = title;
}