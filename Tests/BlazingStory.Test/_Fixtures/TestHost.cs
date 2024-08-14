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
    private readonly IServiceScope? _Scope;

    public TestHost(Action<IServiceCollection>? configureServices = null)
    {
        var services = new ServiceCollection();

        // Mocking IJSRuntime
        services.AddScoped(_ => Mock.Of<IJSRuntime>());

        // NavigationManager setup
        services.AddScoped<TestNavigationManager>();
        services.AddScoped<NavigationManager>(sp => sp.GetRequiredService<TestNavigationManager>());

        // Additional services
        services.AddScoped<HelperScript>();
        services.AddScoped<NavigationService>();
        services.AddScoped(typeof(ILogger<>), typeof(NullLogger<>));

        // Allow external configuration of services
        configureServices?.Invoke(services);

        // Mock IXmlDocComment if not provided
        services.TryAddScoped(_ => Mock.Of<IXmlDocComment>());

        // Build the service provider and create a scope
        this._Scope = services.BuildServiceProvider().CreateScope();
        this.Services = this._Scope.ServiceProvider;

        // Initialize Bunit context and set up its services
        this.BunitContext = new Bunit.TestContext();
        this.BunitContext.Services.AddScoped(_ => this.Services.GetRequiredService<NavigationManager>());
    }

    internal Bunit.TestContext BunitContext { get; private set; }

    internal IServiceProvider Services { get; }

    public async ValueTask DisposeAsync()
    {
        // Dispose the scope if it implements IAsyncDisposable
        if (this._Scope is IAsyncDisposable scope)
        {
            await scope.DisposeAsync();
        }
        else
        {
            _Scope?.Dispose();
        }

        // Dispose Bunit context
        this.BunitContext.Dispose();
    }
}
