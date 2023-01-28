namespace BlazingStory.Internals.Services;

internal class TinyCsvParser
{
    private enum State
    {
        Default,
        InQuote,
        OutQuote
    }

    public static IEnumerable<IReadOnlyList<string>> Parse(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line.Trim())) continue;

            var state = State.Default;
            var colValues = new List<string>();
            var text = new List<char>();
            foreach (var c in line.Append(','))
            {
                switch (state)
                {
                    case State.InQuote:
                        if (c == '"') state = State.OutQuote;
                        else text.Add(c);
                        break;
                    case State.OutQuote:
                        if (c == '"') { state = State.InQuote; text.Add(c); }
                        else if (c == ',') { state = State.Default; colValues.Add(new string(text.ToArray())); text.Clear(); }
                        else throw new FormatException($"Expected ',', but was '{c}'.");
                        break;
                    default:
                        if (c == '"') state = State.InQuote;
                        else if (c == ',') { colValues.Add(new string(text.ToArray())); text.Clear(); }
                        else text.Add(c);
                        break;
                }
            }
            yield return colValues;
        }
    }
}
