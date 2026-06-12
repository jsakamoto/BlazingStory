using System.IO.Compression;
using System.Text;

namespace BlazingStory.Internals.Utils;

public static class UrlParameterShortener
{
    public static string CompressAndEncode(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);

        using (MemoryStream output = new MemoryStream())
        {
            using (DeflateStream gzip = new DeflateStream(output, CompressionLevel.Optimal))
            {
                gzip.Write(bytes, 0, bytes.Length);
            }

            var compressedBytes = output.ToArray();
            return Base64UrlEncode(compressedBytes);
        }
    }

    public static string DecodeAndDecompress(string compressedText)
    {
        var compressedBytes = Base64UrlDecode(compressedText);

        using (MemoryStream input = new MemoryStream(compressedBytes))
        using (MemoryStream output = new MemoryStream())
        {
            using (DeflateStream gzip = new DeflateStream(input, CompressionMode.Decompress))
            {
                gzip.CopyTo(output);
            }

            var decompressedBytes = output.ToArray();
            return Encoding.UTF8.GetString(decompressedBytes);
        }
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }

    private static byte[] Base64UrlDecode(string input)
    {
        var base64 = input.Replace('-', '+').Replace('_', '/');
        var padding = 4 - base64.Length % 4;
        if (padding != 4)
        {
            base64 = base64.PadRight(base64.Length + padding, '=');
        }

        return Convert.FromBase64String(base64);
    }
}
