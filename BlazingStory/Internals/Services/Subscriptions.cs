namespace BlazingStory.Internals.Services;

internal class Subscriptions : IDisposable
{
    #region Private Fields

    private readonly List<IDisposable> _Subscriptions = new();

    #endregion Private Fields

    #region Public Methods

    public void Add(params IDisposable[] subscriptions)
    {
        this._Subscriptions.AddRange(subscriptions);
    }

    public void Dispose()
    {
        foreach (var subscription in this._Subscriptions)
        {
            subscription.Dispose();
        }
    }

    #endregion Public Methods
}
