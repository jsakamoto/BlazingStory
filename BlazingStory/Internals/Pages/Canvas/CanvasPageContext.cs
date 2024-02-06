namespace BlazingStory.Internals.Pages.Canvas;

public class CanvasPageContext
{
    private readonly Dictionary<Type, object?> _Items = new();

    public void SetItem<T>(T item)
    {
        this._Items[typeof(T)] = item;
    }

    public T GetRequiredItem<T>()
    {
        if (!this._Items.TryGetValue(typeof(T), out var item)) throw new InvalidOperationException();
        return (T)item!;
    }
}
