using System.Text.Json;
using BlazingStory.Internals.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingStory.Internals.Utils;

/// <summary>
/// Centralized utility class for processing component event arguments and extracting parameter
/// values. This class handles the complex logic of converting event data from Stencil web
/// components into strongly-typed .NET parameter values, supporting various data formats including
/// direct values, dictionary patterns, and nested object structures. Used by both CanvasFrame and
/// PreviewFrame to ensure consistent event processing across the BlazingStory framework.
///
/// Key capabilities:
/// - Extracts parameter values from component event arguments
/// - Validates two-way binding patterns for component properties
/// - Maps event names to corresponding parameter names using naming conventions
/// - Handles type conversions from JSON/JavaScript types to .NET types
/// - Provides detailed logging for debugging event processing issues
/// </summary>
public static class EventArgumentsProcessor
{
    /// <summary>
    /// Extracts and converts a parameter value from component event arguments, supporting multiple
    /// data formats. This method handles the primary use case of extracting specific parameter
    /// values from Stencil component events, which can come in various formats depending on the
    /// component implementation.
    /// </summary>
    /// <param name="eventArgs">
    /// The event arguments received from a Stencil component event. Can be:
    /// - A dictionary containing key-value pairs (most common)
    /// - A direct value for simple events
    /// - A complex object with nested properties
    /// - A JsonElement from serialized event data
    /// </param>
    /// <param name="parameterName">
    /// The name of the parameter to extract from the event arguments.
    /// Example: "value", "selectedItems", "checkedState" Should match the property name from the
    /// component's @Prop declaration.
    /// </param>
    /// <param name="expectedType">
    /// The expected .NET type for the extracted parameter value. Used for type conversion and
    /// validation. Can be null if type is unknown.
    /// Examples: typeof(string), typeof(int[]), typeof(List&lt;CustomObject&gt;)
    /// </param>
    /// <returns>
    /// The extracted parameter value, converted to the expected type if possible. Returns null if
    /// the parameter cannot be found or converted.
    /// </returns>
    /// <example>
    /// // Extract a string value from event arguments var eventArgs = new Dictionary&lt;string,
    /// object&gt; { ["value"] = "selected option" }; var result = ExtractParameterValue(eventArgs,
    /// "value", typeof(string)); // Returns: "selected option"
    ///
    /// // Extract an array from event arguments var arrayArgs = new Dictionary&lt;string,
    /// object&gt; { ["selectedItems"] = jsonArrayElement }; var arrayResult =
    /// ExtractParameterValue(arrayArgs, "selectedItems", typeof(string[])); // Returns: string[]
    /// containing the selected items
    /// </example>
    public static object? ExtractParameterValue<TArgs>(TArgs eventArgs, string parameterName, Type? expectedType)
    {
        object? value = eventArgs; // Default to the entire event arguments

        // Handle dictionary-based event arguments (most common pattern from Stencil components)
        if (eventArgs is IDictionary<string, object?> dict)
        {
            // First, try to get the parameter value directly from the dictionary
            if (dict.TryGetValue(parameterName, out var directValue) && directValue != null)
            {
                value = directValue;
            }
            // Handle the Keys/Values pattern - some components emit data this way
            else if (dict.TryGetValue("Keys", out var keysObj) && keysObj is JsonElement keysElement && keysElement.ValueKind == JsonValueKind.Array &&
                     dict.TryGetValue("Values", out var valuesObj) && valuesObj is JsonElement valuesElement && valuesElement.ValueKind == JsonValueKind.Array)
            {
                // Extract keys and values arrays for parallel processing
                var keys = keysElement.EnumerateArray().Select(e => e.GetString()).Where(s => s != null).ToList();
                var values = valuesElement.EnumerateArray().Select(e => e.ValueKind == JsonValueKind.String ? e.GetString() : (object)e).ToList();

                // Find the index of our target parameter in the keys array
                var keyIndex = keys.IndexOf(parameterName);
                if (keyIndex >= 0 && keyIndex < values.Count)
                {
                    // Extract the corresponding value using the same index
                    value = values[keyIndex];
                }
            }
        }
        // Handle non-dictionary event arguments (simple value events)
        else if (expectedType == typeof(string) && eventArgs != null)
        {
            // For simple events, treat the entire event args as a string value
            value = eventArgs.ToString();
        }

        // Apply type conversion to ensure the value matches the expected parameter type
        return TypeConversionHelper.ConvertToExpectedType(value, expectedType);
    }

    /// <summary>
    /// Converts an event name to its corresponding parameter name following Stencil two-way binding
    /// conventions. In Stencil components, two-way binding events typically follow the pattern of
    /// PropertyNameChanged, where the event corresponds to a property with the same name minus the
    /// "Changed" suffix.
    /// </summary>
    /// <param name="eventName">
    /// The name of the event emitted by the Stencil component.
    /// Examples: "ValueChanged", "OpenIndexesChanged", "SelectedItemsChanged", "CheckedStateChanged"
    /// </param>
    /// <returns>
    /// The corresponding parameter/property name with the "Changed" suffix removed.
    /// Examples: "Value", "OpenIndexes", "SelectedItems", "CheckedState" If the event name doesn't
    /// end with "Changed", returns the original event name.
    /// </returns>
    /// <example>
    /// string paramName1 = MapEventNameToParameterName("ValueChanged"); // Returns: "Value" string
    /// paramName2 = MapEventNameToParameterName("ItemsChanged"); // Returns: "Items" string
    /// paramName3 = MapEventNameToParameterName("CustomEvent"); // Returns: "CustomEvent" (no
    /// "Changed" suffix)
    /// </example>
    public static string MapEventNameToParameterName(string eventName)
    {
        // Check if this follows the standard two-way binding convention
        return eventName.EndsWith("Changed")
            ? eventName.Replace("Changed", "") // Remove "Changed" suffix to get property name
            : eventName;                       // Return as-is for non-standard events
    }

    /// <summary>
    /// Validates that a component event follows proper Stencil two-way binding patterns. This
    /// method ensures that for every XxxChanged event, there exists a corresponding Xxx property in
    /// the component's parameter definitions, which is required for proper two-way data binding.
    /// </summary>
    /// <param name="eventName">
    /// The name of the event to validate. Should follow the pattern "PropertyNameChanged" for
    /// two-way binding events. Examples: "ValueChanged", "SelectedItemsChanged"
    /// </param>
    /// <param name="story">
    /// The story object containing the component's parameter definitions and metadata. Used to look
    /// up whether the corresponding property exists. Can be null for validation bypass.
    /// </param>
    /// <param name="contextPrefix">
    /// Logging context identifier for debugging purposes. Helps identify which component or frame
    /// is performing the validation. Default: "EventProcessor"
    /// </param>
    /// <returns>
    /// true if the event follows proper two-way binding patterns (ends with "Changed" and has
    /// corresponding property); false if the event doesn't follow the pattern or the corresponding
    /// property is missing.
    /// </returns>
    /// <example>
    /// // Validate a proper two-way binding event var story = GetStoryWithValueProperty(); // Story
    /// has "Value" parameter defined bool isValid1 = ValidateTwoWayBindingPattern("ValueChanged",
    /// story, "MyComponent"); // Returns: true (event has "Changed" suffix and "Value" property exists)
    ///
    /// // Validate an invalid two-way binding event bool isValid2 =
    /// ValidateTwoWayBindingPattern("MissingPropertyChanged", story, "MyComponent"); // Returns:
    /// false (no "MissingProperty" parameter defined in story)
    ///
    /// // Validate a non-two-way-binding event bool isValid3 =
    /// ValidateTwoWayBindingPattern("ButtonClicked", story, "MyComponent"); // Returns: false
    /// (doesn't end with "Changed")
    /// </example>
    public static bool ValidateTwoWayBindingPattern(string eventName, Story? story, string contextPrefix = "EventProcessor")
    {
        // First check: Does the event follow the "XxxChanged" naming convention?
        if (!eventName.EndsWith("Changed"))
        {
            return false;
        }

        // Second check: Do we have story context and parameter definitions to validate against?
        if (story?.Context?.Parameters == null)
        {
            return false;
        }

        // Extract the expected property name by removing "Changed" suffix
        var parameterName = MapEventNameToParameterName(eventName);

        // Third check: Does the corresponding property actually exist in the component?
        var propertyParameter = story.Context.Parameters.FirstOrDefault(p => p.Name == parameterName);
        if (propertyParameter == null)
        {
            // KEEP this log - it's critical for identifying missing property configurations
            Console.WriteLine($"{contextPrefix}: Two-way binding validation FAILED - Property '{parameterName}' not found for event '{eventName}'");
            return false;
        }

        // Fourth check: Does the event callback parameter exist in the story?
        var eventParameter = story.Context.Parameters.FirstOrDefault(p => p.Name == eventName);
        if (eventParameter == null)
        {
            // KEEP this log - it's critical for identifying missing event configurations
            Console.WriteLine($"{contextPrefix}: Two-way binding validation FAILED - Event '{eventName}' parameter not found");
            return false;
        }

        // Fifth check: Is the event parameter actually an EventCallback type? This ensures proper
        // Blazor event handling capabilities
        if (!eventParameter.Type.IsAssignableTo(typeof(EventCallback)) &&
            !(eventParameter.Type.IsGenericType && eventParameter.Type.GetGenericTypeDefinition() == typeof(EventCallback<>)))
        {
            // KEEP this log - it's critical for identifying type configuration errors
            Console.WriteLine($"{contextPrefix}: Two-way binding validation FAILED - '{eventName}' is not an EventCallback type");
            return false;
        }

        // Sixth check: Validate type compatibility between property and event for strongly-typed events
        if (eventParameter.Type.IsGenericType && eventParameter.Type.GetGenericTypeDefinition() == typeof(EventCallback<>))
        {
            var eventGenericType = eventParameter.Type.GenericTypeArguments.FirstOrDefault();
            var propertyType = propertyParameter.TypeStructure.PrimaryType;

            if (eventGenericType != null && propertyType != null)
            {
                // Check for exact type match first (most common and ideal case)
                if (eventGenericType != propertyType)
                {
                    // Check for compatible types (e.g., array vs IList, nullable vs non-nullable)
                    if (!AreTypesCompatibleForTwoWayBinding(propertyType, eventGenericType))
                    {
                        // KEEP this log - it's critical for identifying type mismatch errors
                        Console.WriteLine($"{contextPrefix}: Two-way binding validation FAILED - Type mismatch between property '{parameterName}' ({propertyType.Name}) and event '{eventName}' ({eventGenericType.Name})");
                        return false;
                    }
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Finds the parameter definition for a given parameter name in the story context.
    /// </summary>
    /// <param name="parameterName">
    /// The parameter name to find
    /// </param>
    /// <param name="story">
    /// The story containing the parameter definitions
    /// </param>
    /// <returns>
    /// The parameter definition or null if not found
    /// </returns>
    public static ComponentParameter? FindParameter(string parameterName, Story? story)
    {
        if (story?.Context?.Parameters == null)
        {
            return null;
        }

        var parameter = story.Context.Parameters.FirstOrDefault(p => p.Name == parameterName);
        return parameter;
    }

    /// <summary>
    /// Checks if two types are compatible for two-way binding scenarios. Handles common type
    /// variations like arrays vs lists, nullable vs non-nullable, etc.
    /// </summary>
    /// <param name="propertyType">
    /// The property type
    /// </param>
    /// <param name="eventType">
    /// The event callback generic type
    /// </param>
    /// <returns>
    /// True if the types are compatible for two-way binding
    /// </returns>
    private static bool AreTypesCompatibleForTwoWayBinding(Type propertyType, Type eventType)
    {
        // Exact match
        if (propertyType == eventType)
            return true;

        // Handle nullable types
        var propertyNullableUnderlying = Nullable.GetUnderlyingType(propertyType);
        var eventNullableUnderlying = Nullable.GetUnderlyingType(eventType);

        if (propertyNullableUnderlying != null || eventNullableUnderlying != null)
        {
            var propertyCore = propertyNullableUnderlying ?? propertyType;
            var eventCore = eventNullableUnderlying ?? eventType;
            return propertyCore == eventCore;
        }

        // Handle array vs IList/ICollection/IEnumerable variations
        if (propertyType.IsArray && eventType.IsGenericType)
        {
            var propertyElementType = propertyType.GetElementType();
            var eventGenericDef = eventType.GetGenericTypeDefinition();

            if ((eventGenericDef == typeof(IList<>) ||
                 eventGenericDef == typeof(ICollection<>) ||
                 eventGenericDef == typeof(IEnumerable<>) ||
                 eventGenericDef == typeof(List<>)) &&
                eventType.GenericTypeArguments.FirstOrDefault() == propertyElementType)
            {
                return true;
            }
        }

        // Handle IList vs array variations (reverse case)
        if (eventType.IsArray && propertyType.IsGenericType)
        {
            var eventElementType = eventType.GetElementType();
            var propertyGenericDef = propertyType.GetGenericTypeDefinition();

            if ((propertyGenericDef == typeof(IList<>) ||
                 propertyGenericDef == typeof(ICollection<>) ||
                 propertyGenericDef == typeof(IEnumerable<>) ||
                 propertyGenericDef == typeof(List<>)) &&
                propertyType.GenericTypeArguments.FirstOrDefault() == eventElementType)
            {
                return true;
            }
        }

        // Handle generic collection variations (List<T> vs IList<T>, etc.)
        if (propertyType.IsGenericType && eventType.IsGenericType)
        {
            var propertyGenericDef = propertyType.GetGenericTypeDefinition();
            var eventGenericDef = eventType.GetGenericTypeDefinition();
            var propertyArgs = propertyType.GenericTypeArguments;
            var eventArgs = eventType.GenericTypeArguments;

            // Check if they have the same generic arguments
            if (propertyArgs.Length == eventArgs.Length &&
                propertyArgs.SequenceEqual(eventArgs))
            {
                // Common collection interface compatibility
                var compatibleTypes = new[]
                {
                    typeof(IList<>), typeof(ICollection<>), typeof(IEnumerable<>),
                    typeof(List<>), typeof(IReadOnlyList<>), typeof(IReadOnlyCollection<>)
                };

                if (compatibleTypes.Contains(propertyGenericDef) &&
                    compatibleTypes.Contains(eventGenericDef))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
