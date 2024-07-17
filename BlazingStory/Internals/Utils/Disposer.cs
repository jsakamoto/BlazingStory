namespace BlazingStory.Internals.Utils;

internal class Disposer : IDisposable
{
    #region Private Fields

    private readonly Action _Callback;

    #endregion Private Fields

    #region Public Constructors

    public Disposer(Action callback)
    {
        this._Callback = callback;
    }

    #endregion Public Constructors

    #region Public Methods

    public void Dispose() => this._Callback.Invoke();

    #endregion Public Methods
}
