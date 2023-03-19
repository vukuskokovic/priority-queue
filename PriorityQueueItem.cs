namespace PriorityQueue;
internal class PriorityQueueItem<TValue, TPriority>
{
    internal readonly TValue Value;
    internal readonly TPriority Pririty;
    internal PriorityQueueItem(TValue value, TPriority priority)
    {
        Value = value;
        Pririty = priority;
    }
}
