namespace BlazingStory.Internals.Services;

internal class Subscriptions : IDisposable
{
    private readonly List<IDisposable> _Subscriptions = new();

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
}
