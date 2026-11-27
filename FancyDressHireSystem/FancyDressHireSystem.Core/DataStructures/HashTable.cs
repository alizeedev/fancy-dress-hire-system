namespace FancyDressHireSystem.Core.DataStructures
{
    public class HashTable<TKey, TValue>
    {
        private HashNode<TKey, TValue>?[] _buckets;
        private int _capacity;
        private int _count;

        public int Count
        {
            get { return _count; }
        }

        public HashTable()
        {
            _capacity = 16;
            _buckets = new HashNode<TKey, TValue>?[_capacity];
            _count = 0;
        }
    }
}