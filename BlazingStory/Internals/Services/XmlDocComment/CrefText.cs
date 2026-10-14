using BlazingStory.ToolKit.Utils;

namespace BlazingStory.Internals.Services.XmlDocComment;

/// <summary>
/// Converts a "cref" attribute value of an XML document comment - a documentation identifier
/// such as <c>P:Foo.MySelect`2.Multiselect</c> - into a text for display.
/// </summary>
internal static class CrefText
{
    /// <summary>
    /// Get a text for display from a "cref" attribute value of an XML document comment.<br/>
    /// (e.g. <c>P:Foo.MySelect`2.Multiselect</c> =&gt; <c>MySelect&lt;TKey, TValue&gt;.Multiselect</c>)
    /// </summary>
    /// <param name="cref">The value of a "cref" attribute.</param>
    /// <param name="contextType">
    /// The type that owns the XML document comment. It is used for resolving the names of generic type parameters,
    /// which a documentation identifier records as an arity only. It can be null.
    /// </param>
    public static string GetDisplayText(string cref, Type? contextType)
    {
        if (string.IsNullOrEmpty(cref)) return "";

        var hasPrefix = cref.Length > 1 && cref[1] == ':';
        var identifier = TrimParameterList(hasPrefix ? cref[2..] : cref);

        // A documentation identifier of a type or a namespace is fully qualified, so only its last segment is the
        // name itself; for a member, the segment before its name is the type that declares it, which is worth showing.
        var segmentCount = hasPrefix && (cref[0] == 'T' || cref[0] == 'N') ? 1 : 2;

        return string.Join('.', SplitTopLevel(identifier, '.')
            .TakeLast(segmentCount)
            .Select(segment => GetSegmentDisplayText(segment, contextType)));
    }

    /// <summary>
    /// Trim the parameter list of a method or an indexer, to keep the text short enough to read in a sentence.<br/>
    /// (e.g. <c>Foo.Bar(System.String)</c> =&gt; <c>Foo.Bar</c>)
    /// </summary>
    private static string TrimParameterList(string identifier)
    {
        var index = identifier.IndexOf('(');
        return index == -1 ? identifier : identifier[..index];
    }

    /// <summary>
    /// Split a documentation identifier by the given separator, except for the separators
    /// that are inside generic type arguments.<br/>
    /// (e.g. <c>Foo.Bar{System.String}.Fizz</c> =&gt; <c>Foo</c>, <c>Bar{System.String}</c>, <c>Fizz</c>)
    /// </summary>
    private static IEnumerable<string> SplitTopLevel(string identifier, char separator)
    {
        var depth = 0;
        var startAt = 0;
        for (var i = 0; i < identifier.Length; i++)
        {
            var c = identifier[i];
            if (c == '{') depth++;
            else if (c == '}') depth--;
            else if (c == separator && depth == 0)
            {
                yield return identifier[startAt..i];
                startAt = i + 1;
            }
        }
        yield return identifier[startAt..];
    }

    /// <summary>
    /// Get a text for display from a single segment of a documentation identifier, by rewriting its generic
    /// type arguments into the C# notation.<br/>
    /// (e.g. <c>MySelect`2</c> =&gt; <c>MySelect&lt;TKey, TValue&gt;</c>, <c>List{System.String}</c> =&gt; <c>List&lt;String&gt;</c>)
    /// </summary>
    private static string GetSegmentDisplayText(string segment, Type? contextType)
    {
        var openBraceAt = segment.IndexOf('{');
        var closeBraceAt = segment.LastIndexOf('}');
        if (openBraceAt != -1 && closeBraceAt > openBraceAt)
        {
            var typeArguments = SplitTopLevel(segment[(openBraceAt + 1)..closeBraceAt], ',')
                .Select(typeArgument => GetSegmentDisplayText(SplitTopLevel(typeArgument, '.').Last(), contextType));
            return GetSegmentDisplayText(segment[..openBraceAt], contextType) +
                "<" + string.Join(", ", typeArguments) + ">" +
                segment[(closeBraceAt + 1)..];
        }

        // A backtick in a documentation identifier always introduces a generic arity. (e.g. the `2 of MySelect`2)
        var arityAt = segment.IndexOf('`');
        if (arityAt == -1) return segment;

        var name = segment[..arityAt];
        var typeParameterNames = GetTypeParameterNames(contextType, segment);
        return typeParameterNames == null ? name : name + "<" + string.Join(", ", typeParameterNames) + ">";
    }

    /// <summary>
    /// Get the names of the generic type parameters of the type that the given segment of a documentation
    /// identifier refers to. (e.g. <c>MySelect`2</c> =&gt; <c>TKey</c>, <c>TValue</c>)<br/>
    /// A documentation identifier records the arity of a generic type, but not the names of its type parameters,
    /// so the names can be recovered only when the referred type is the type that owns the XML document comment
    /// or one of its base types. It returns null when they can not be recovered.
    /// </summary>
    private static string[]? GetTypeParameterNames(Type? contextType, string nameWithArity)
    {
        for (var type = contextType; type != null; type = type.BaseType)
        {
            var openType = TypeUtility.GetOpenType(type);
            if (openType.IsGenericTypeDefinition && openType.Name == nameWithArity)
            {
                return openType.GetGenericArguments().Select(arg => arg.Name).ToArray();
            }
        }
        return null;
    }
}
