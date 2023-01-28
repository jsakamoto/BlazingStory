using BlazingStory.Internals.Services;

namespace BlazingStory.Test.Internals.Services;

internal class TinyCsvParserTest
{
    [Test]
    public void Parse_Test()
    {
        var lines = new[] {
            "Key,Code",
            "Alt,",
            ",AltLeft",
            ",",
            "A,KeyA",
            "\",\",Comma"
        };

        var rows = TinyCsvParser.Parse(lines.Skip(1));
        rows.Select(row => string.Join('|', row)).Is(
            "Alt|",
            "|AltLeft",
            "|",
            "A|KeyA",
            ",|Comma");
    }
}
