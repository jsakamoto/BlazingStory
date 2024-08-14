namespace BlazingStory.Internals.Utils;

internal class Disposer : IDisposable
{
    private readonly Action _Callback;

    public Disposer(Action callback)
    {
        this._Callback = callback;
    }

    public void Dispose() => this._Callback.Invoke();
}
