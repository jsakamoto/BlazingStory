using Toolbelt.Blazor.HotKeys2;

namespace BlazingStory.Internals.Services;

internal class KeyCodeMapper
{
    private static Dictionary<string, string>? _CodeToKey;

    private static IReadOnlyDictionary<string, string> GetCodeToKeyMap()
    {
        if (_CodeToKey == null)
        {
            var csvLines = EnnumKeyCodeMapCsvLines();
            var keyCodePair = TinyCsvParser.Parse(csvLines.Skip(1));
            _CodeToKey = keyCodePair
                .Where(pair => pair.Count() >= 2 && !string.IsNullOrEmpty(pair[1]))
                .ToDictionary(pair => pair[1], pair => pair[0]);
        }
        return _CodeToKey;
    }

    private static IEnumerable<string> EnnumKeyCodeMapCsvLines()
    {
        using var stream = typeof(KeyCodeMapper).Assembly.GetManifestResourceStream("BlazingStory.Resources.KeyCodeMap.csv");
        if (stream == null) yield break;
        using var streamReader = new StreamReader(stream);
        while (!streamReader.EndOfStream)
        {
            yield return streamReader.ReadLine() ?? "";
        }
    }

    public static string GetKeyTextFromCode(Code code)
    {
        var codeToKeyMap = GetCodeToKeyMap();
        return codeToKeyMap.TryGetValue(code, out var key) ? key : code;
    }
}
