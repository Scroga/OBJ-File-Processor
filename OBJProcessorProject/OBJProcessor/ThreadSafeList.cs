using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace OBJProcessor;

public class ThreadSafeEnumerator<T> : IEnumerator<T>
{
    private readonly IEnumerator<T> _inner;
    private readonly object _lock;

    public ThreadSafeEnumerator(IEnumerator<T> inner, object @lock)
    {
        _inner = inner;
        _lock = @lock;
        Monitor.Enter(_lock);
    }

    public T Current => _inner.Current;
    object System.Collections.IEnumerator.Current => Current!;


    public bool MoveNext()
    {
        return _inner.MoveNext();
    }

    public void Reset()
    {
        _inner.Reset();
    }

    public void Dispose()
    {
        Monitor.Exit(_lock);
    }
}

public class ThreadSafeList<T> : IList<T>
{
    private readonly List<T> _inner;
    private readonly object _lock = new();

    public ThreadSafeList()
    {
        _inner = new();
    }

    public int Count
    {
        get
        {
            lock (_lock)
            {
                return _inner.Count;
            }
        }
    }
    public bool IsReadOnly
    {
        get
        {
            return false;
        }
    }

    public T this[int index]
    {
        get
        {
            lock (_lock)
            {
                return _inner[index];
            }
        }
        set
        {
            lock (_lock)
            {
                _inner[index] = value;
            }
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        lock (_lock)
        {
            return new ThreadSafeEnumerator<T>(_inner.GetEnumerator(), _lock);
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        lock (_lock)
        {
            return new ThreadSafeEnumerator<T>(_inner.GetEnumerator(), _lock);
        }
    }

    public void Add(T item)
    {
        lock (_lock)
        {
            _inner.Add(item);
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _inner.Clear();
        }
    }

    public bool Contains(T item)
    {
        lock (_lock)
        {
            return _inner.Contains(item);
        }
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        lock (_lock)
        {
            _inner.CopyTo(array, arrayIndex);
        }
    }

    public bool Remove(T item)
    {
        lock (_lock)
        {
            return _inner.Remove(item);
        }
    }

    public int IndexOf(T item)
    {
        lock (_lock)
        {
            return _inner.IndexOf(item);
        }
    }

    public void Insert(int index, T item)
    {
        lock (_lock)
        {
            _inner.Insert(index, item);
        }
    }

    public void RemoveAt(int index)
    {
        lock (_lock)
        {
            _inner.RemoveAt(index);
        }
    }

    public ReadOnlyCollection<T> AsReadOnly()
    {
        lock (_lock)
        {
            return _inner.AsReadOnly();
        }
    }

    public void ForEach(Action<T> action)
    {
        lock (_lock)
        {
            _inner.ForEach(action);
        }
    }

    public bool Exists(Predicate<T> match)
    {
        lock (_lock)
        {
            return _inner.Exists(match);
        }
    }
}
