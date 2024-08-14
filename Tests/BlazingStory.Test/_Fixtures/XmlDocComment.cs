using BlazingStory.Internals.Services.XmlDocComment;
using Moq;

namespace BlazingStory.Test._Fixtures;

/// <summary>
/// Helper class for mocking <see cref="IXmlDocComment" />.
/// </summary>
internal static class XmlDocComment
{
    /// <summary>
    /// Dummy instance of <see cref="IXmlDocComment" />.
    /// </summary>
    internal static IXmlDocComment Dummy => Mock.Of<IXmlDocComment>();
}
