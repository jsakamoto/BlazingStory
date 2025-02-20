using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace BlazingStory.Internals.Utils;

/// <summary>
/// Provides methods for serializing objects to JSON without any exceptions even when the object contains unsupported types or cyclic references.
/// </summary>
internal class JsonFallbackSerializer
{
    /// <summary>
    /// Serializes the specified object to a JSON string.<br/>
    /// If the property value is an unsupported type, the property value is serialized as a string that indicates the type is unsupported.<br/>
    /// The cyclic references are detected and serialized as a string that indicates the cyclic references are not supported.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="configure">An optional action to configure the <see cref="JsonSerializerOptions"/>.</param>
    /// <returns>A JSON string representation of the object.</returns>
    internal static string Serialize<[DynamicallyAccessedMembers(PublicProperties)] T>(T value, Action<JsonSerializerOptions>? configure = null)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters = { new JsonFallbackConverter<T>() }
        };
        configure?.Invoke(options);

#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
        return JsonSerializer.Serialize(value, options);
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
    }
}
