using System.Collections.Generic;

public class LimitedStack<T>
{
    private readonly int _capacity;
    private readonly LinkedList<T> _list = new();

    public int Count => _list.Count;

    public LimitedStack(int capacity) { _capacity = capacity; }

    public void Push(T item)
    {
        _list.AddFirst(item);
        if (_list.Count > _capacity)
            _list.RemoveLast(); // descartar el más viejo
    }

    public T Pop()
    {
        var first = _list.First;
        _list.RemoveFirst();
        return first.Value;
    }

    public void Clear() => _list.Clear();
}
