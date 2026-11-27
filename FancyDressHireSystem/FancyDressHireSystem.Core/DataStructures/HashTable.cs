namespace FancyDressHireSystem.Core.DataStructures
{
    // Our own hash table: an array of drawers, each holding a chain of HashNode cards
    public class HashTable<TValue>
    {
        private HashNode<string, TValue>?[] _buckets;
        private int _capacity;
        private int _count;

        public int Count
        {
            get { return _count; }
        }

        public HashTable()
        {
            _capacity = 16;
            _buckets = new HashNode<string, TValue>?[_capacity];
            _count = 0;
        }
    }
}