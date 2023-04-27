namespace BlazingStory.Internals.Services.XmlDocComment;

public interface IXmlDocComment
{
    ValueTask<string> GetSummaryOfPropertyAsync(Type ownerType, string propertyName);
}
