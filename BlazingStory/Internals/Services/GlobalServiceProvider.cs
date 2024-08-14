namespace BlazingStory.Internals.Services;

internal class GlobalServiceProvider : IServiceProvider
{
    private readonly IServiceProvider _ServiceProvider;

    internal GlobalServiceProvider(IServiceProvider serviceProvider)
    {
        this._ServiceProvider = serviceProvider;
    }

    public object? GetService(Type serviceType) => this._ServiceProvider.GetService(serviceType);
}
