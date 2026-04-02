using System.Net;
using System.Text;
using BlazingStory.Internals.Pages.Settings.Panels;
using BlazingStory.Test._Fixtures;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingStory.Test.Internals.Pages.Settings.Panels;

internal class AboutPanelTest
{
    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _StatusCode;
        private readonly string _Content;

        public FakeHttpMessageHandler(HttpStatusCode statusCode, string content)
        {
            this._StatusCode = statusCode;
            this._Content = content;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(this._StatusCode)
            {
                Content = new StringContent(this._Content, Encoding.UTF8, "application/json")
            });
        }
    }

    [Test]
    public async Task Render_WithContributors_ShowsAvatars_Test()
    {
        // Given
        var json = """
        [
            {"login":"user1","avatar_url":"https://example.com/avatar1.png","html_url":"https://github.com/user1"},
            {"login":"user2","avatar_url":"https://example.com/avatar2.png","html_url":"https://github.com/user2"}
        ]
        """;
        var httpClient = new HttpClient(new FakeHttpMessageHandler(HttpStatusCode.OK, json))
        {
            BaseAddress = new Uri("http://localhost/")
        };

        await using var host = new TestHost(services =>
        {
            services.AddSingleton(httpClient);
        });

        using var ctx = new BunitContext();
        ctx.RenderTree.Add<CascadingValue<IServiceProvider>>(p =>
            p.Add(p => p.Value, host.Services));

        // When
        var cut = ctx.Render<AboutPanel>();

        // Then - contributors section is rendered with 2 avatars
        var section = cut.Find(".contributors-section");
        var avatarLinks = section.QuerySelectorAll(".contributors-grid a");
        avatarLinks.Count.Is(2);
        avatarLinks[0].GetAttribute("href").Is("https://github.com/user1");
        avatarLinks[0].GetAttribute("title").Is("user1");
        avatarLinks[1].GetAttribute("href").Is("https://github.com/user2");

        var images = section.QuerySelectorAll(".contributors-grid img");
        images[0].GetAttribute("src").Is("https://example.com/avatar1.png");
        images[0].GetAttribute("alt").Is("user1");
        images[1].GetAttribute("src").Is("https://example.com/avatar2.png");

        var countBadge = section.QuerySelector(".contributors-count");
        countBadge!.TextContent.Trim().Is("2");
    }

    [Test]
    public async Task Render_WithApiFailure_HidesContributorsSection_Test()
    {
        // Given
        var httpClient = new HttpClient(new FakeHttpMessageHandler(HttpStatusCode.Forbidden, "{}"))
        {
            BaseAddress = new Uri("http://localhost/")
        };

        await using var host = new TestHost(services =>
        {
            services.AddSingleton(httpClient);
        });

        using var ctx = new BunitContext();
        ctx.RenderTree.Add<CascadingValue<IServiceProvider>>(p =>
            p.Add(p => p.Value, host.Services));

        // When
        var cut = ctx.Render<AboutPanel>();

        // Then - header still shows, no contributors section
        cut.Find("header").TextContent.Contains("Blazing Story").IsTrue();
        cut.FindAll(".contributors-section").Count.Is(0);
    }

    [Test]
    public async Task Render_WithEmptyResponse_HidesContributorsSection_Test()
    {
        // Given
        var httpClient = new HttpClient(new FakeHttpMessageHandler(HttpStatusCode.OK, "[]"))
        {
            BaseAddress = new Uri("http://localhost/")
        };

        await using var host = new TestHost(services =>
        {
            services.AddSingleton(httpClient);
        });

        using var ctx = new BunitContext();
        ctx.RenderTree.Add<CascadingValue<IServiceProvider>>(p =>
            p.Add(p => p.Value, host.Services));

        // When
        var cut = ctx.Render<AboutPanel>();

        // Then - no contributors section
        cut.Find("header").TextContent.Contains("Blazing Story").IsTrue();
        cut.FindAll(".contributors-section").Count.Is(0);
    }
}
