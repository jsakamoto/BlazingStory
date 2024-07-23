namespace BlazingStory.Internals.Pages.Canvas;

public class CanvasPageContext
{
    #region Private Fields

    private readonly Dictionary<Type, object?> _Items = new();

    #endregion Private Fields

    #region Public Methods

    public void SetItem<T>(T item)
    {
        this._Items[typeof(T)] = item;
    }

    public T GetRequiredItem<T>()
    {
        if (!this._Items.TryGetValue(typeof(T), out var item))
        {
            throw new InvalidOperationException();
        }

        return (T)item!;
    }

    #endregion Public Methods
}
