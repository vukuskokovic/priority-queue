using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PriorityQueue;

public class PriorityQueue<TValue, TPriority> : IEnumerable<TValue>, IEnumerable where TPriority : IComparable
{
    private readonly object _lockObject = new();
    private readonly List<PriorityQueueItem<TValue, TPriority>> _itemList = new();

    public int Count => _itemList.Count;


    /// <summary>
    /// Removes an item from the queue based on its idnex
    /// </summary>
    /// <param name="index">Index to remove </param>
    /// <exception cref="ArgumentOutOfRangeException">If the index is non positive or out of range</exception>
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index));

        _itemList.RemoveAt(index);
    }
    /// <summary>
    /// Gets the index of the first occurance of the item
    /// </summary>
    /// <param name="item">The item to search for</param>
    /// <returns>Index or <c>-1</c> if not found</returns>
    public int IndexOf(TValue item)
    {
        int index = 0;
        foreach (var queueItem in _itemList) {
            if (queueItem.Equals(item)) return index;

            index++;
        }

        return -1;
    }

    /// <summary>
    /// Adds an item to the queue
    /// </summary>
    /// <param name="item">The item to enqueue</param>
    /// <param name="priroity">The priority of the item</param>
    /// <returns>Index of the enqueded item</returns>
    /// <exception cref="ArgumentNullException">If the priority item is null</exception>
    public int Enqueue(TValue item, TPriority priroity = default)
    {
        if (priroity == null) throw new ArgumentNullException(nameof(priroity), "Priority value cannot be null");

        lock (_lockObject)
        {
            PriorityQueueItem<TValue, TPriority> newItem = new(item, priroity);

            int index = 0;
            foreach(var queueItem in _itemList)
            {
                if(queueItem.Pririty.CompareTo(priroity) < 0)
                {
                    index++;
                    continue;
                }

                break;
            }
            _itemList.Insert(index, newItem);
            return index;
        }
    }

    /// <summary>
    /// Tries to dequeue an item from the queue
    /// </summary>
    /// <param name="item">The item dequeued</param>
    /// <returns>The sucess of the dequeue</returns>
    public bool TryDequeue(out TValue item)
    {
        lock (_lockObject)
        {
            if (_itemList.Count == 0)
            {
                item = default;
                return false;
            }

            item = _itemList[0].Value;
            _itemList.RemoveAt(0);
            return true;
        }
    }

    /// <summary>
    /// Dequeues an item
    /// </summary>
    /// <returns>The dequeued item</returns>
    /// <exception cref="InvalidOperationException">If the queue is empty</exception>
    public TValue Dequeue()
    {
        lock( _lockObject)
        {
            if (_itemList.Count == 0) throw new InvalidOperationException("Cannot dequeue an empty queue");

            var firstItem = _itemList[0].Value;
            _itemList.RemoveAt(0);
            return firstItem;
        }
    }

    public IEnumerator<TValue> GetEnumerator()
    {
        return _itemList.Select(x => x.Value).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}