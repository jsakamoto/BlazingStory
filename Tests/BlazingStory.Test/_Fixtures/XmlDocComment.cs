using BlazingStory.Internals.Services.XmlDocComment;
using NSubstitute;

namespace BlazingStory.Test._Fixtures;

/// <summary>
/// Helper class for mocking <see cref="IXmlDocComment"/>.
/// </summary>
internal static class XmlDocComment
{
    /// <summary>
    /// Dummy instance of <see cref="IXmlDocComment"/>.
    /// </summary>
    internal static IXmlDocComment Dummy => Substitute.For<IXmlDocComment>();
}
