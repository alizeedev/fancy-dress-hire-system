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
        
        // Adds a new item. Throws an error if the key is already used
        public void Add(string key, TValue value)
        {
            int index = Hash(key);
            HashNode<string, TValue>? node = _buckets[index];

            while (node != null)
            {
                if (node.Key == key)
                {
                    throw new ArgumentException("Key already exists: " + key);
                }
                node = node.Next;
            }

            HashNode<string, TValue> newNode = new HashNode<string, TValue>(key, value);
            newNode.Next = _buckets[index];
            _buckets[index] = newNode;
            _count = _count + 1;

            // Resize check goes here once Resize() exists
        }
        

    }
}