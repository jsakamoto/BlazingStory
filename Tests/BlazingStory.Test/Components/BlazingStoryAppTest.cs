using BlazingStory.Components;
using BlazingStory.Services;
using BlazingStory.Test._Fixtures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using NSubstitute;
using System.Reflection;

namespace BlazingStory.Test.Components;

internal class BlazingStoryAppTest
{
    [Test]
    public void ConfigureServices_registers_action_logger_Test()
    {
        // Given
        var app = new BlazingStoryApp();
        SetPrivateProperty(app, "JSRuntime", Substitute.For<IJSRuntime>());
        SetPrivateProperty(app, "LoggerFactory", LoggerFactory.Create(_ => { }));
        SetPrivateProperty(app, "NavigationManager", new TestNavigationManager());
        SetPrivateProperty(app, "GlobalServices", new ServiceCollection()
            .AddSingleton(new HttpClient())
            .BuildServiceProvider());

        var services = new ServiceCollection();

        // When
        var configureServices = typeof(BlazingStoryApp).GetMethod("ConfigureServices", BindingFlags.Instance | BindingFlags.NonPublic);
        configureServices.IsNotNull();
        configureServices!.Invoke(app, [services]);

        var actionLoggerDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IBlazingStoryActionLogger));
        var serviceProvider = services.BuildServiceProvider();
        var resolvedLogger = serviceProvider.GetService<IBlazingStoryActionLogger>();

        // Then
        actionLoggerDescriptor.IsNotNull();
        actionLoggerDescriptor!.Lifetime.Is(ServiceLifetime.Scoped);
        actionLoggerDescriptor.ImplementationType.Is(typeof(BlazingStoryActionLogger));
        resolvedLogger.IsNotNull();
    }

    private static void SetPrivateProperty(object instance, string propertyName, object value)
    {
        var property = instance.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        property.IsNotNull();
        property!.SetValue(instance, value);
    }
}
