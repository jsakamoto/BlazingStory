using System.Globalization;
using System.Text;

namespace BlazingStory.Internals.Pages.TableOfContents;

/// <summary>
/// Builds table-of-contents models from heading sources.
/// </summary>
internal static class TableOfContentsModelFactory
{
    /// <summary>
    /// Creates table-of-contents items from source headings.
    /// </summary>
    /// <param name="headings">The source headings.</param>
    /// <param name="minHeadingLevel">The minimum included heading level.</param>
    /// <param name="maxHeadingLevel">The maximum included heading level.</param>
    /// <returns>The root table-of-contents items.</returns>
    internal static List<TableOfContentsItem> Create(
        IEnumerable<TableOfContentsSourceHeading> headings,
        int minHeadingLevel = TableOfContentsDefaults.MinHeadingLevel,
        int maxHeadingLevel = TableOfContentsDefaults.MaxHeadingLevel)
    {
        var (normalizedMinHeadingLevel, normalizedMaxHeadingLevel) =
            TableOfContentsDefaults.NormalizeHeadingLevelRange(minHeadingLevel, maxHeadingLevel);

        var roots = new List<TableOfContentsItem>();
        var stack = new Stack<TableOfContentsItem>();
        var idCounter = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var heading in headings)
        {
            if (!TableOfContentsDefaults.IsIncludedHeadingLevel(heading.Level, normalizedMinHeadingLevel, normalizedMaxHeadingLevel)) continue;

            var baseId = !string.IsNullOrWhiteSpace(heading.Id) ? Slugify(heading.Id!) : Slugify(heading.Text);
            var uniqueId = EnsureUniqueId(baseId, idCounter);
            var item = new TableOfContentsItem(uniqueId, heading.Text, heading.Level);

            while (stack.Count > 0 && stack.Peek().Level >= item.Level)
            {
                stack.Pop();
            }

            if (stack.Count == 0) roots.Add(item);
            else stack.Peek().Children.Add(item);

            stack.Push(item);
        }

        return roots;
    }

    /// <summary>
    /// Converts text into a URL-friendly slug.
    /// </summary>
    /// <param name="source">The source text.</param>
    /// <returns>A slug string.</returns>
    internal static string Slugify(string source)
    {
        if (string.IsNullOrWhiteSpace(source)) return "section";

        var normalized = OperatingSystem.IsBrowser()
            ? source
            : source.Normalize(NormalizationForm.FormKD);
        var sb = new StringBuilder(capacity: normalized.Length);
        var previousWasDash = false;

        foreach (var ch in normalized)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (unicodeCategory == UnicodeCategory.NonSpacingMark) continue;

            if (char.IsLetterOrDigit(ch))
            {
                sb.Append(char.ToLowerInvariant(ch));
                previousWasDash = false;
                continue;
            }

            if (previousWasDash) continue;
            sb.Append('-');
            previousWasDash = true;
        }

        var slug = sb.ToString().Trim('-');
        return string.IsNullOrWhiteSpace(slug) ? "section" : slug;
    }

    /// <summary>
    /// Ensures a generated slug is unique within the current heading set.
    /// </summary>
    /// <param name="baseId">The base slug.</param>
    /// <param name="idCounter">The id counter map.</param>
    /// <returns>A unique slug id.</returns>
    private static string EnsureUniqueId(string baseId, IDictionary<string, int> idCounter)
    {
        if (!idCounter.TryGetValue(baseId, out var count))
        {
            idCounter[baseId] = 1;
            return baseId;
        }

        count++;
        idCounter[baseId] = count;
        return $"{baseId}-{count}";
    }
}