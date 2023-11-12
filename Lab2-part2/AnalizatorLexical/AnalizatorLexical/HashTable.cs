namespace AnalizatorLexical;

public class HashTable<T>
{
    private const int INITIAL_CAPACITY = 13;
    protected readonly T[] _slots;

    public int Count { get; private set; }
    public IEnumerable<T> Items
    {
        get
        {
            return _slots.Where(elem => elem != null);
        }
    }

    public HashTable()
    {
        _slots = new T[INITIAL_CAPACITY];
    }

    protected virtual int GetHash(T item)
    {
        if (item == null) 
            throw new ArgumentNullException(nameof(item));
        
        int hash = item.GetHashCode();
        return Math.Abs(hash % _slots.Length);
    }

    /// <summary>
    /// Adds an element to the hashtable
    /// </summary>
    /// <param name="item"> the element to be added </param>
    /// <returns> the position where the element was added </returns>
    /// <exception cref="InvalidOperationException"> if the hashtable is full </exception>
    public int Add(T item)
    {
        int index = GetHash(item);

        if (_slots[index] == null)
        {
            _slots[index] = item;
            Count++;
            return index;
        }

        int newIndex = (index + 1) % _slots.Length;
        while (newIndex != index)
        {
            if (_slots[newIndex] == null)
            {
                _slots[newIndex] = item;
                return newIndex;
            }
            newIndex = (newIndex + 1) % _slots.Length;
        }

        throw new InvalidOperationException("Hashtable is full.");
    }

    /// <summary>
    /// Checks if the hashtable contains a given element.
    /// </summary>
    /// <param name="item"> the element to check if it is contained in the hashtable </param>
    /// <returns> the index of the element if it contains the element, -1 otherwise </returns>
    public int Contains(T item)
    {
        
        int index = GetHash(item);

        if (_slots[index] != null && item!.Equals(_slots[index]))
        {
            return index;
        }

        int newIndex = (index + 1) % _slots.Length;
        while (newIndex != index)
        {
            if (_slots[newIndex] == null)
            {
                return -1;
            }
            if (item!.Equals(_slots[newIndex]))
            {
                return newIndex;
            }
            
            newIndex = (newIndex + 1) % _slots.Length;
        }
        
        return -1;
    }
}