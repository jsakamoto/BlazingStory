using BlazingStory.Internals.Services.XmlDocComment;
using NSubstitute;

namespace BlazingStory.Test.Shared._Fixtures;

/// <summary>
/// Helper class for mocking <see cref="IXmlDocComment"/>.
/// </summary>
public static class XmlDocComment
{
    /// <summary>
    /// Dummy instance of <see cref="IXmlDocComment"/>.
    /// </summary>
    public static IXmlDocComment Dummy => Substitute.For<IXmlDocComment>();
}
