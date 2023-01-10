using System.Globalization;
using System.Text.RegularExpressions;

namespace BlazingStory.Internals.Services;

internal class WebColor
{
    internal enum Type
    {
        Unknown,
        Hex,
        RGBA,
        HSLA
    }

    internal readonly double R;

    internal readonly double G;

    internal readonly double B;

    internal readonly double H;

    internal readonly double S;

    internal readonly double L;

    internal readonly double A;

    internal readonly string AlphaText;

    internal readonly string HexOrNameText;

    internal readonly string RGBAText;

    internal readonly string HSLAText;

    private WebColor(double r, double g, double b, double h, double s, double l, double a, string alphaText, string hexOrNameText, string rgbaText, string hslaText)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.H = h;
        this.S = s;
        this.L = l;
        this.A = a;
        this.AlphaText = alphaText;
        this.HexOrNameText = hexOrNameText;
        this.RGBAText = rgbaText;
        this.HSLAText = hslaText;
    }

    internal static (bool success, WebColor? color, Type type) Parse(string colorText)
    {
        colorText = colorText.Trim();

        var matchHex = Regex.Match(colorText, @"(^#(?<R1>[0-9a-f]{2})(?<G1>[0-9a-f]{2})(?<B1>[0-9a-f]{2})$)|(^#(?<R2>[0-9a-f])(?<G2>[0-9a-f])(?<B2>[0-9a-f])$)", RegexOptions.IgnoreCase);
        if (matchHex.Success)
        {
            static int hex2int(string hex) => int.Parse(hex, NumberStyles.HexNumber);
            var r = hex2int(matchHex.Groups["R1"].Value + matchHex.Groups["R2"].Value + matchHex.Groups["R2"].Value);
            var g = hex2int(matchHex.Groups["G1"].Value + matchHex.Groups["G2"].Value + matchHex.Groups["G2"].Value);
            var b = hex2int(matchHex.Groups["B1"].Value + matchHex.Groups["B2"].Value + matchHex.Groups["B2"].Value);
            return (true, WebColor.FromHex(colorText, r, g, b), Type.Hex);
        }

        static (double A, string AText) extractAlpha(Match m)
        {
            var aText = m.Groups["A"].Success ? m.Groups["A"].Value : "1";
            var a = double.Parse(aText.TrimEnd('%'));
            if (aText.EndsWith('%')) a /= 100.0;
            return (a, aText);
        }

        var matchRgba = Regex.Match(colorText, @"^rgba?\((?<R>\d+)[ ,]+(?<G>\d+)[ ,]+(?<B>\d+)([ ,]+(?<A>[\d\.]+%?))?\)", RegexOptions.IgnoreCase);
        if (matchRgba.Success)
        {
            var r = double.Parse(matchRgba.Groups["R"].Value);
            var g = double.Parse(matchRgba.Groups["G"].Value);
            var b = double.Parse(matchRgba.Groups["B"].Value);
            var (a, alphaText) = extractAlpha(matchRgba);
            return (true, WebColor.FromRGBA(colorText, r, g, b, a, alphaText), Type.RGBA);
        }

        var matchHsla = Regex.Match(colorText, @"^hsla?\((?<H>\d+)[ ,]+(?<S>\d+)%[ ,]+(?<L>\d+)%([ ,]+(?<A>[\d\.]+%?))?\)", RegexOptions.IgnoreCase);
        if (matchHsla.Success)
        {
            var h = double.Parse(matchHsla.Groups["H"].Value);
            var s = double.Parse(matchHsla.Groups["S"].Value);
            var l = double.Parse(matchHsla.Groups["L"].Value);
            var (a, alphaText) = extractAlpha(matchHsla);
            return (true, WebColor.FromHSLA(colorText, h, s, l, a, alphaText), Type.HSLA);
        }

        return (false, null, Type.Unknown);
    }

    private static WebColor FromHex(string hexText, double r, double g, double b)
    {
        var (h, s, l) = RGBtoHSL(r, g, b);
        var rgbaText = BuildRGBAText(r, g, b, "1");
        var hslaText = BuildHSLAText(h, s, l, "1");
        return new WebColor(r, g, b, h, s, l, 1.0, "1", hexText, rgbaText, hslaText);
    }

    private static WebColor FromRGBA(string rgbaText, double r, double g, double b, double a, string alphaText)
    {
        var (h, s, l) = RGBtoHSL(r, g, b);
        var hexText = BuildHexText(r, g, b, a);
        var hslaText = BuildHSLAText(h, s, l, alphaText);
        return new WebColor(r, g, b, h, s, l, a, alphaText, hexText, rgbaText, hslaText);
    }

    private static WebColor FromHSLA(string hslaText, double h, double s, double l, double a, string alphaText)
    {
        var (r, g, b) = HSLtoRGB(h, s, l);
        var hexText = BuildHexText(r, g, b, a);
        var rgbaText = BuildRGBAText(r, g, b, alphaText);
        return new WebColor(r, g, b, h, s, l, a, alphaText, hexText, rgbaText, hslaText);
    }

    private static string BuildHexText(double r, double g, double b, double a)
    {
        static string num2hex(double n) => ((byte)Math.Min(Math.Round(n), 255)).ToString("x2");
        if (a == 1.0)
            return $"#{num2hex(r)}{num2hex(g)}{num2hex(b)}";
        else
        {
            var bytea = 255 * a;
            return $"#{num2hex(r)}{num2hex(g)}{num2hex(b)}{num2hex(bytea)}";
        }
    }

    private static string BuildRGBAText(double r, double g, double b, string alphaText)
    {
        static byte num2byte(double n) => ((byte)Math.Min(Math.Round(n), 255));
        return $"rgba({num2byte(r)}, {num2byte(g)}, {num2byte(b)}, {alphaText})";
    }

    private static string BuildHSLAText(double h, double s, double l, string alhpaText)
    {
        return $"hsla({h}, {s}%, {l}%, {alhpaText})";
    }

    private static (double H, double S, double L) RGBtoHSL(double r, double g, double b)
    {
        r /= 255.0;
        g /= 255.0;
        b /= 255.0;

        // https://www.programmingalgorithms.com/algorithm/rgb-to-hsl/
        var min = Math.Min(Math.Min(r, g), b);
        var max = Math.Max(Math.Max(r, g), b);
        var delta = max - min;

        var h = Math.Round(min == max ? 0 :
            min == r ? (60 * ((b - g) / delta)) + 180 :
            min == g ? (60 * ((r - b) / delta)) + 300 :
            min == b ? (60 * ((g - r) / delta)) + 60 : 0);

        var s = Math.Round(delta / (1 - (Math.Abs(max + min - 1))) * 100);
        var l = Math.Round((max + min) / 2 * 100);
        return (h, s, l);
    }

    private static (byte R, byte G, byte B) HSLtoRGB(double h, double s, double l)
    {
        h /= 360.0;
        s /= 100.0;
        l /= 100.0;
        static byte toByte(double n) => (byte)Math.Min(Math.Round(255 * n), 255);

        // https://www.programmingalgorithms.com/algorithm/hsl-to-rgb/
        if (s == 0)
        {
            var n = toByte(l);
            return (n, n, n);
        }
        else
        {
            var v2 = (l < 0.5) ? (l * (1 + s)) : ((l + s) - (l * s));
            var v1 = 2 * l - v2;

            static double hueToRGB(double v1, double v2, double vH)
            {
                if (vH < 0) vH += 1;
                if (vH > 1) vH -= 1;
                if ((6 * vH) < 1) return (v1 + (v2 - v1) * 6 * vH);
                if ((2 * vH) < 1) return v2;
                if ((3 * vH) < 2) return (v1 + (v2 - v1) * ((2.0f / 3) - vH) * 6);
                return v1;
            }

            return (
                toByte(hueToRGB(v1, v2, h + (1.0f / 3))),
                toByte(hueToRGB(v1, v2, h)),
                toByte(hueToRGB(v1, v2, h - (1.0f / 3)))
            );
        }
    }
}
