using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.Navigation;
using BlazingStory.Internals.Services.XmlDocComment;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.JSInterop;
using Moq;

namespace BlazingStory.Test._Fixtures;

internal class TestHost : IAsyncDisposable
{
    private readonly IServiceScope _Scope;

    private readonly Bunit.TestContext _bUnitContext;

    internal IServiceProvider Services { get; }

    public TestHost(Action<IServiceCollection>? configureServices = null)
    {
        this._bUnitContext = new Bunit.TestContext();

        var services = new ServiceCollection();
        services.AddScoped(_ => Mock.Of<IJSRuntime>());
        services.AddScoped<NavigationManager>(_ => new Bunit.TestDoubles.FakeNavigationManager(this._bUnitContext));
        services.AddScoped<HelperScript>();
        services.AddScoped<NavigationService>();
        services.AddScoped(typeof(ILogger<>), typeof(NullLogger<>));
        configureServices?.Invoke(services);
        services.TryAddScoped(_ => Mock.Of<IXmlDocComment>());

        this._Scope = services.BuildServiceProvider().CreateScope();
        this.Services = this._Scope.ServiceProvider;
    }

    public async ValueTask DisposeAsync()
    {
        if (this._Scope is IAsyncDisposable scope) await scope.DisposeAsync();
        this._bUnitContext.Dispose();
    }
}
