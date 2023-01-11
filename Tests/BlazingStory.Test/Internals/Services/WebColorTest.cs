using BlazingStory.Internals.Services;

namespace BlazingStory.Test.Internals.Services;

internal class WebColorTest
{
    private static string DumpAlpha(WebColor color) => $"A:{color.A}, AText:{color.AlphaText}";

    private static string DumpHSL(WebColor color) => $"H:{color.H}, S:{color.S}, L:{color.L}";

    private static string DumpRGB(WebColor color) => $"R:{color.R}, G:{color.G}, B:{color.B}";

    [Test]
    public void Parse_HexText_Test()
    {
        var (color, type) = WebColor.Parse("#EBA4E8");
        color.IsNotNull();
        type.Is(WebColor.Type.Hex);

        DumpAlpha(color).Is("A:1, AText:1");

        DumpHSL(color).Is("H:303, S:64, L:78");
        color.HSLAText.Is("hsla(303, 64%, 78%, 1)");

        DumpRGB(color).Is("R:235, G:164, B:232");
        color.RGBAText.Is("rgba(235, 164, 232, 1)");

        color.HexOrNameText.Is("#EBA4E8");
    }

    [Test]
    public void Parse_HexText_Short_Test()
    {
        var (color, type) = WebColor.Parse("#f3c");
        color.IsNotNull();
        type.Is(WebColor.Type.Hex);

        DumpAlpha(color).Is("A:1, AText:1");

        DumpHSL(color).Is("H:315, S:100, L:60");
        color.HSLAText.Is("hsla(315, 100%, 60%, 1)");

        DumpRGB(color).Is("R:255, G:51, B:204");
        color.RGBAText.Is("rgba(255, 51, 204, 1)");

        color.HexOrNameText.Is("#f3c");
    }

    [Test]
    public void Parse_RGBAText_Test()
    {
        var (color, type) = WebColor.Parse("rgba(235, 164, 232, 30%)");
        color.IsNotNull();
        type.Is(WebColor.Type.RGBA);

        DumpAlpha(color).Is("A:0.3, AText:30%");

        DumpHSL(color).Is("H:303, S:64, L:78");
        color.HSLAText.Is("hsla(303, 64%, 78%, 30%)");

        DumpRGB(color).Is("R:235, G:164, B:232");
        color.RGBAText.Is("rgba(235, 164, 232, 30%)");

        color.HexOrNameText.Is("#eba4e84c");
    }

    [Test]
    public void Parse_HSLAText_Test()
    {
        var (color, type) = WebColor.Parse("hsl(303, 64%, 78%, 0.3)");
        color.IsNotNull();
        type.Is(WebColor.Type.HSLA);

        DumpAlpha(color).Is("A:0.3, AText:0.3");

        DumpHSL(color).Is("H:303, S:64, L:78");
        color.HSLAText.Is("hsl(303, 64%, 78%, 0.3)");

        DumpRGB(color).Is("R:235, G:163, B:231");
        color.RGBAText.Is("rgba(235, 163, 231, 0.3)");

        color.HexOrNameText.Is("#eba3e74c");
    }
}
