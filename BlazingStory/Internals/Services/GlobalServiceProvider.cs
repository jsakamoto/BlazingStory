namespace BlazingStory.Internals.Services;

internal class GlobalServiceProvider : IServiceProvider
{
    #region Private Fields

    private readonly IServiceProvider _ServiceProvider;

    #endregion Private Fields

    #region Internal Constructors

    internal GlobalServiceProvider(IServiceProvider serviceProvider)
    {
        this._ServiceProvider = serviceProvider;
    }

    #endregion Internal Constructors

    #region Public Methods

    public object? GetService(Type serviceType) => this._ServiceProvider.GetService(serviceType);

    #endregion Public Methods
}
