using System.Text.Json;

namespace BlazingStory.Internals.Utils;

/// <summary>
/// Helper class for converting values to expected types in BlazingStory component interactions.
/// This class consolidates type conversion logic used by both CanvasFrame and PreviewFrame when
/// processing component property values from events and URL parameters.
///
/// Handles complex scenarios like:
/// - JSON element deserialization
/// - Array/List conversions between different collection types
/// - Primitive type parsing from strings
/// - Null value handling
/// </summary>
public static class TypeConversionHelper
{
    /// <summary>
    /// Converts a value to the expected type, handling common conversions encountered in Blazor
    /// component property binding scenarios.
    ///
    /// This method is designed to handle values coming from various sources:
    /// - JSON deserialization (JsonElement objects)
    /// - URL query parameters (strings)
    /// - JavaScript interop (mixed object types)
    /// - Component event arguments (dynamic objects)
    /// </summary>
    /// <param name="value">
    /// The value to convert (can be null, JsonElement, string, or any object)
    /// </param>
    /// <param name="expectedType">
    /// The target type to convert to (component property type)
    /// </param>
    /// <returns>
    /// The converted value, or the original value if no conversion is needed/possible
    /// </returns>
    /// <example>
    /// // Convert JsonElement array to string array var result = ConvertToExpectedType(jsonElement, typeof(string[]));
    ///
    /// // Convert string to int var result = ConvertToExpectedType("42", typeof(int));
    /// </example>
    public static object? ConvertToExpectedType(object? value, Type? expectedType)
    {
        // Early exit optimization: no conversion needed if value is null or type is unknown
        if (value == null || expectedType == null)
            return value;

        // Fast path for string conversions - most common case for simple text properties
        if (expectedType == typeof(string))
        {
            return value.ToString();
        }

        // Collection type handling - critical for component properties that expect arrays or lists
        // This covers scenarios like selectedItems: string[], openIndexes: number[], etc.
        if (expectedType.IsArray || (expectedType.IsGenericType && expectedType.GetGenericTypeDefinition() == typeof(IList<>)))
        {
            return ConvertToArrayOrList(value, expectedType);
        }

        // Primitive type conversions from strings - essential for URL parameter processing These
        // handle the common case where component properties come from query string values

        // Integer conversion - handles numeric component properties from URLs
        if (expectedType == typeof(int) && value is string intStr && int.TryParse(intStr, out var intValue))
        {
            return intValue; // "42" → 42
        }

        // Boolean conversion - handles toggle/checkbox states from URLs
        if (expectedType == typeof(bool) && value is string boolStr && bool.TryParse(boolStr, out var boolValue))
        {
            return boolValue; // "true" → true, "false" → false
        }

        // Double precision conversion - handles decimal properties from URLs
        if (expectedType == typeof(double) && value is string doubleStr && double.TryParse(doubleStr, out var doubleValue))
        {
            return doubleValue; // "3.14159" → 3.14159
        }

        // Single precision conversion - handles float properties from URLs
        if (expectedType == typeof(float) && value is string floatStr && float.TryParse(floatStr, out var floatValue))
        {
            return floatValue; // "2.5" → 2.5f
        }

        // High-precision decimal conversion - handles currency/financial values from URLs
        if (expectedType == typeof(decimal) && value is string decimalStr && decimal.TryParse(decimalStr, out var decimalValue))
        {
            return decimalValue; // "99.99" → 99.99m
        }

        // Long integer conversion - handles large numeric values from URLs
        if (expectedType == typeof(long) && value is string longStr && long.TryParse(longStr, out var longValue))
        {
            return longValue; // "1234567890123" → 1234567890123L
        }

        // Fallback: return original value if no specific conversion applies This preserves complex
        // objects and handles direct type compatibility
        return value;
    }

    /// <summary>
    /// Performs deep equality comparison between two values by serializing them to JSON and
    /// comparing the results. This method is essential for detecting actual value changes in
    /// component two-way binding scenarios, particularly when dealing with complex objects, arrays,
    /// or values that may have different reference equality but represent the same logical data.
    /// </summary>
    /// <param name="currentValue">
    /// The current/existing value to compare. Can be any serializable object including:
    /// - Primitive types (int, string, bool, etc.)
    /// - Arrays and collections
    /// - Complex objects with properties
    /// - null values
    /// </param>
    /// <param name="newValue">
    /// The new/incoming value to compare against the current value. Should be of compatible type or
    /// represent the same logical data structure as currentValue.
    /// </param>
    /// <returns>
    /// true if both values serialize to identical JSON representations, indicating they contain the
    /// same data; false if the values differ or if serialization fails for either value.
    /// </returns>
    /// <example>
    /// // Primitive values bool equal1 = AreValuesEqual(42, 42); // Returns true bool equal2 =
    /// AreValuesEqual("hello", "world"); // Returns false
    ///
    /// // Array comparison var array1 = new[] { "a", "b", "c" }; var array2 = new[] { "a", "b", "c"
    /// }; bool equal3 = AreValuesEqual(array1, array2); // Returns true (same content)
    ///
    /// // Object comparison var obj1 = new { Name = "John", Age = 30 }; var obj2 = new { Name =
    /// "John", Age = 30 }; bool equal4 = AreValuesEqual(obj1, obj2); // Returns true (same
    /// properties and values)
    /// </example>
    public static bool AreValuesEqual(object? currentValue, object? newValue)
    {
        // Serialize both values to JSON for deep comparison This handles complex objects, arrays,
        // and nested structures automatically
        var currentValueJson = JsonSerializer.Serialize(currentValue);
        var newValueJson = JsonSerializer.Serialize(newValue);

        // Compare the JSON strings - if they're identical, the values represent the same data
        return currentValueJson == newValueJson;
    }

    /// <summary>
    /// Converts various value types to strongly-typed arrays or lists, with special handling for
    /// JsonElement arrays. This method is the main entry point for collection conversions in the
    /// type system.
    /// </summary>
    /// <param name="value">
    /// The value to convert to a collection. Can be:
    /// - JsonElement with array data (most common case from component events)
    /// - Existing array or list that needs type conversion
    /// - String that might represent a serialized array
    /// - Any other object that should be converted to a single-element collection
    /// </param>
    /// <param name="expectedType">
    /// The target collection type. Must be either:
    /// - Array type (e.g., string[], int[], CustomClass[])
    /// - Generic List type (e.g., List&lt;string&gt;, List&lt;int&gt;, List&lt;CustomClass&gt;)
    /// </param>
    /// <returns>
    /// A strongly-typed collection matching expectedType, or null if conversion fails. The returned
    /// object can be safely cast to the expectedType.
    /// </returns>
    /// <example>
    /// // Convert JsonElement array from component event JsonElement jsonArray =
    /// GetFromComponentEvent(); // Contains ["option1", "option2", "option3"] var result =
    /// ConvertToArrayOrList(jsonArray, typeof(string[])); // Returns string[] {"option1",
    /// "option2", "option3"}
    ///
    /// // Convert to generic list var listResult = ConvertToArrayOrList(jsonArray,
    /// typeof(List&lt;string&gt;)); // Returns List&lt;string&gt; containing the same items
    /// </example>
    private static object? ConvertToArrayOrList(object? value, Type expectedType)
    {
        // Handle JsonElement arrays - the most common case for component event data
        if (value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
        {
            // Determine the element type and whether we're creating an array or list
            var elementType = expectedType.IsArray ? expectedType.GetElementType() : expectedType.GetGenericArguments()[0];
            var isArray = expectedType.IsArray;

            // Handle common primitive types with optimized conversions
            if (elementType == typeof(string))
            {
                // Convert JSON array elements to strings, filtering out null values
                var enumerable = jsonElement.EnumerateArray().Select(e => e.GetString()).Where(s => s != null);
                return isArray ? enumerable.ToArray() : enumerable.ToList();
            }
            else if (elementType == typeof(int))
            {
                // Convert JSON array elements to integers
                var enumerable = jsonElement.EnumerateArray().Select(e => e.GetInt32());
                return isArray ? enumerable.ToArray() : enumerable.ToList();
            }
            else if (elementType == typeof(double))
            {
                // Convert JSON array elements to doubles
                var enumerable = jsonElement.EnumerateArray().Select(e => e.GetDouble());
                return isArray ? enumerable.ToArray() : enumerable.ToList();
            }
            else if (elementType == typeof(long))
            {
                // Convert JSON array elements to long integers
                var enumerable = jsonElement.EnumerateArray().Select(e => e.GetInt64());
                return isArray ? enumerable.ToArray() : enumerable.ToList();
            }
            else if (elementType == typeof(float))
            {
                // Convert JSON array elements to floats (cast from double since JSON uses double precision)
                var enumerable = jsonElement.EnumerateArray().Select(e => (float)e.GetDouble());
                return isArray ? enumerable.ToArray() : enumerable.ToList();
            }
            else if (elementType == typeof(decimal))
            {
                // Convert JSON array elements to decimals for high-precision calculations
                var enumerable = jsonElement.EnumerateArray().Select(e => e.GetDecimal());
                return isArray ? enumerable.ToArray() : enumerable.ToList();
            }
            else if (elementType == typeof(bool))
            {
                // Convert JSON array elements to booleans
                var enumerable = jsonElement.EnumerateArray().Select(e => e.GetBoolean());
                return isArray ? enumerable.ToArray() : enumerable.ToList();
            }
        }
        // Handle existing .NET collections that might need type conversion
        else if (value is IList<object> list)
        {
            return list; // Pass through existing collections as-is
        }

        // Fallback: return the original value if no conversion is possible
        return value;
    }
}
