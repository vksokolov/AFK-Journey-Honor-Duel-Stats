using System.Collections;
using System.Collections.Generic;

namespace Utils
{
    public interface IReadOnlyHashSet<T> : IEnumerable<T>
    {
        bool Contains(T item);
        int Count { get; }
    }
    
    public class ReadOnlyHashSet<T> : IReadOnlyHashSet<T>
    {
        private readonly HashSet<T> _hashSet;

        public ReadOnlyHashSet(HashSet<T> hashSet)
        {
            _hashSet = hashSet;
        }

        public bool Contains(T item) => _hashSet.Contains(item);
        public int Count => _hashSet.Count;
        public IEnumerator<T> GetEnumerator() => 
            _hashSet.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }
}