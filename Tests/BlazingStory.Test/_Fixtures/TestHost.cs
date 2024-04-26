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

    internal Bunit.TestContext BunitContext { get; private set; }

    internal IServiceProvider Services { get; }

    public TestHost(Action<IServiceCollection>? configureServices = null)
    {
        var services = new ServiceCollection();
        services.AddScoped(_ => Mock.Of<IJSRuntime>());
        services.AddScoped<TestNavigationManager>();
        services.AddScoped<NavigationManager>(sp => sp.GetRequiredService<TestNavigationManager>());
        services.AddScoped<HelperScript>();
        services.AddScoped<NavigationService>();
        services.AddScoped(typeof(ILogger<>), typeof(NullLogger<>));
        configureServices?.Invoke(services);
        services.TryAddScoped(_ => Mock.Of<IXmlDocComment>());

        this._Scope = services.BuildServiceProvider().CreateScope();
        this.Services = this._Scope.ServiceProvider;

        this.BunitContext = new Bunit.TestContext();
        this.BunitContext.Services.AddScoped(_ => this.Services.GetRequiredService<NavigationManager>());
    }

    public async ValueTask DisposeAsync()
    {
        if (this._Scope is IAsyncDisposable scope) await scope.DisposeAsync();
        this.BunitContext.Dispose();
    }
}
