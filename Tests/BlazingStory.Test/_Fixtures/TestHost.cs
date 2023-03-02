using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;

namespace BlazingStory.Test._Fixtures;

internal class TestHost : IAsyncDisposable
{
    private IServiceScope _Scope;

    internal IServiceProvider Services { get; }

    public TestHost(Action<IServiceCollection>? configureServices = null)
    {
        var services = new ServiceCollection();
        services.AddScoped<IJSRuntime>(_ => new Mock<IJSRuntime>().Object);
        services.AddScoped<NavigationManager>(_ => new Mock<NavigationManager>().Object);
        services.AddScoped<HelperScript>();
        services.AddScoped<NavigationService>();
        configureServices?.Invoke(services);

        this._Scope = services.BuildServiceProvider().CreateScope();
        this.Services = this._Scope.ServiceProvider;
    }

    public async ValueTask DisposeAsync()
    {
        var scope = this._Scope as IAsyncDisposable;
        if (scope != null) await scope.DisposeAsync();
    }
}
