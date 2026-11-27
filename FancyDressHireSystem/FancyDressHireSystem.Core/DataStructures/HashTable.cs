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

        // Our own hash function: turns a key like "GA001" into a drawer number
        private int Hash(string key)
        {
            int total = 0;
            foreach (char c in key)
            {
                total = (total * 31 + (int)c) % _capacity;
            }

            return total;
        }

    }
}