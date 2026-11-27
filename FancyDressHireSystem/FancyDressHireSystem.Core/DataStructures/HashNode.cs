namespace FancyDressHireSystem.Core.DataStructures
{
    public class HashNode< TKey , TValue >
    {
        public TKey Key { get; }
        public TValue Value { get; set; }
        public HashNode<TKey, TValue>? Next { get; set; }

        public HashNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Next = null;
        }
    }
}