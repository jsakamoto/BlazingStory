using System.Net;

namespace BlazingStory.Test._Fixtures;

/// <summary>
/// This class is used to load XML documentation file regarding the assembly of the specified type.
/// </summary>
internal class XmlDocCommentLoader
{
    /// <summary>
    /// This class is used to load XML documentation file regarding the assembly of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the assembly to load XML documentation file.</typeparam>
    private class XmlDocCommentMessageHandler<T> : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var xdocPath = Path.ChangeExtension(new Uri(typeof(T).Assembly.Location).LocalPath, ".xml");
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(File.ReadAllText(xdocPath)) });
        }
    }

    /// <summary>
    /// Creates a <see cref="HttpClient"/> that loads XML documentation file regarding the assembly of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the assembly to load XML documentation file.</typeparam>
    /// <returns>A <see cref="HttpClient"/> that loads XML documentation file regarding the assembly of the specified type.</returns>
    public static HttpClient CreateHttpClientFor<T>()
    {
        return new HttpClient(new XmlDocCommentMessageHandler<T>()) { BaseAddress = new Uri("http://localhost/") };
    }
}
