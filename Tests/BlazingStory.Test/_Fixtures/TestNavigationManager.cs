using Microsoft.AspNetCore.Components;

namespace BlazingStory.Test._Fixtures;

internal class TestNavigationManager : NavigationManager
{
    public TestNavigationManager()
    {
        this.Initialize("http://localhost/", "http://localhost/");
    }

    internal void SetUrls(string baseUri, string uri)
    {
        this.BaseUri = baseUri;
        this.Uri = uri;
    }

    protected override void NavigateToCore(string uri, NavigationOptions options)
    {
        this.Uri = this.ToAbsoluteUri(uri).ToString();
    }
}
