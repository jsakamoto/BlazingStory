namespace BlazingStory.Internals.Utils.TextSearch;

internal class TextSearchFragment
{
    internal TextSearchFragmentType Type;

    internal string Text;

    public TextSearchFragment(TextSearchFragmentType type, string text)
    {
        this.Type = type;
        this.Text = text;
    }

    internal static IEnumerable<TextSearchFragment> CreateFragments(string? text, IEnumerable<string> keywords)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return new[] { new TextSearchFragment(TextSearchFragmentType.None, string.Empty) };
        }

        // Find match ranges
        var matchRanges = new List<(int Begin, int End)>();

        foreach (var keyword in keywords)
        {
            var startIndex = 0;

            for (; ; )
            {
                var begin = text.IndexOf(keyword, startIndex, StringComparison.OrdinalIgnoreCase);

                if (begin == -1)
                {
                    break;
                }

                matchRanges.Add((begin, begin + keyword.Length));
                startIndex = begin + 1;
            }
        }

        // Normalize the match ranges
        var sourceMatchRanges = matchRanges.OrderBy(range => range.Begin).ToArray();
        matchRanges.Clear();

        foreach (var range in sourceMatchRanges)
        {
            var overwrapedRangeIndex = matchRanges.FindIndex(r1 => !(r1.End < range.Begin || range.End < r1.Begin));

            if (overwrapedRangeIndex == -1)
            {
                matchRanges.Add(range);
            }
            else
            {
                var overwrapedRange = matchRanges[overwrapedRangeIndex];
                matchRanges[overwrapedRangeIndex] = (
                    Begin: Math.Min(range.Begin, overwrapedRange.Begin),
                    End: Math.Max(range.End, overwrapedRange.End));
            }
        }

        // Build fragments
        var unmatchPosBegin = 0;
        var fragments = new List<TextSearchFragment>();

        foreach (var range in matchRanges)
        {
            var unmatchPosEnd = range.Begin;
            var len = unmatchPosEnd - unmatchPosBegin;

            if (len > 0)
            {
                fragments.Add(new(TextSearchFragmentType.None, text.Substring(unmatchPosBegin, len)));
            }

            fragments.Add(new(TextSearchFragmentType.Match, text.Substring(range.Begin, range.End - range.Begin)));

            unmatchPosBegin = range.End;
        }

        if (unmatchPosBegin < text.Length)
        {
            fragments.Add(new(TextSearchFragmentType.None, text.Substring(unmatchPosBegin)));
        }

        return fragments.Any() ? fragments : new[] { new TextSearchFragment(TextSearchFragmentType.None, text) };
    }
}
