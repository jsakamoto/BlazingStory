namespace BlazingStory.Internals.Services.XmlDocComment;

/// <summary>
/// Provides XML document comment of types
/// </summary>
public interface IXmlDocComment
{
    /// <summary>
    /// Get summary text of a property from XML document comment file.
    /// </summary>
    /// <param name="ownerType">Type of the property owner.</param>
    /// <param name="propertyName">Name of the property.</param>
    ValueTask<string> GetSummaryOfPropertyAsync(Type ownerType, string propertyName);

    /// <summary>
    /// Get summary text of a type from XML document comment file.
    /// </summary>
    /// <param name="componentType">Type for getting summary text.</param>
    ValueTask<string> GetSummaryOfTypeAsync(Type componentType);
}
