using System.Net;

namespace BlazingStory.Test._Fixtures;

/// <summary>
/// This class is used to load XML documentation file from the test output directory.
/// </summary>
internal class XmlDocCommentLoaderFromOutDir
{
    /// <summary>
    /// Creates a <see cref="HttpClient" /> for test that loads XML documentation file from the test
    /// output directory..
    /// </summary>
    /// <returns>
    /// A <see cref="HttpClient" /> for test that loads XML documentation file from the test output directory.
    /// </returns>
    public static HttpClient CreateHttpClient()
    {
        return new HttpClient(new XmlDocCommentMessageHandler()) { BaseAddress = new Uri("http://localhost/") };
    }

    /// <summary>
    /// This class is used to load XML documentation from the test output directory.
    /// </summary>
    private class XmlDocCommentMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var outDir = Path.GetDirectoryName(new Uri(this.GetType().Assembly.Location).LocalPath) ?? throw new NullReferenceException();
            var fileName = Path.GetFileName(request.RequestUri?.LocalPath) ?? throw new NullReferenceException();
            var xdocPath = Path.Combine(outDir, fileName);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(File.ReadAllText(xdocPath)) });
        }
    }
}
