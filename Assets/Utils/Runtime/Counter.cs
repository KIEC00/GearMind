using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Utils.Runtime
{
    public interface IReadOnlyCounter<T> : IEnumerable<KeyValuePair<T, int>>
        where T : notnull
    {
        int Total { get; }
        int Count { get; }
        int this[T key] { get; }
        Dictionary<T, int>.KeyCollection Keys { get; }
    }

    public class Counter<T> : IReadOnlyCounter<T>, IEnumerable<KeyValuePair<T, int>>
        where T : notnull
    {
        public Dictionary<T, int>.KeyCollection Keys => _counter.Keys;
        public int Total => _counter.Values.Sum();
        public int Count => _counter.Count;
        public bool IsEmpty => _counter.Count == 0;
        public int this[T key]
        {
            get => Get(key);
            set => Set(key, value);
        }

        private readonly Dictionary<T, int> _counter = new();

        public Counter() { }

        public Counter(IEnumerable<T> collection) => Join(collection);

        public Counter(Counter<T> other) => _counter = new(other._counter);

        public Counter<T> Add(in T item, int addCount = 1)
        {
            _counter.TryGetValue(item, out int count);
            _counter[item] = count + addCount;
            return this;
        }

        public Counter<T> Join(IEnumerable<(T, int)> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), "value must not be null.");
            foreach (var (obj, count) in collection)
                Add(obj, count);
            return this;
        }

        public Counter<T> Join(IEnumerable<T> collection) =>
            Join(collection?.Select(obj => (obj, 1)));

        public Counter<T> Join(IEnumerable<KeyValuePair<T, int>> collection) =>
            Join(collection?.Select(pair => (pair.Key, pair.Value)));

        public Counter<T> Join(Counter<T> other) =>
            Join(other?.Select(pair => (pair.Key, pair.Value)));

        public Counter<T> Remove(in T item, int removeCount = 1)
        {
            if (!_counter.TryGetValue(item, out int count))
                return this;
            if (count > removeCount)
                _counter[item] = count - removeCount;
            else
                _counter.Remove(item);
            return this;
        }

        public Counter<T> Subtract(IEnumerable<(T, int)> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), "value must not be null.");
            foreach (var (obj, count) in collection)
                Remove(obj, count);
            return this;
        }

        public Counter<T> Subtract(IEnumerable<T> collection) =>
            Subtract(collection?.Select(obj => (obj, 1)));

        public Counter<T> Subtract(IEnumerable<KeyValuePair<T, int>> collection) =>
            Subtract(collection?.Select(pair => (pair.Key, pair.Value)));

        public Counter<T> Subtract(Counter<T> other) =>
            Subtract(other?.Select(pair => (pair.Key, pair.Value)));

        public Counter<T> Multiply(int multiply)
        {
            if (multiply < 0)
                throw new ArgumentOutOfRangeException(nameof(multiply), "value must be >= 0.");
            if (multiply == 0)
                Clear();
            if (multiply <= 1)
                return this;
            foreach (var key in _counter.Keys.ToArray())
                _counter[key] *= multiply;
            return this;
        }

        public IEnumerable<KeyValuePair<T, int>> MostCommon() =>
            _counter.OrderByDescending(pair => pair.Value);

        public IEnumerator<KeyValuePair<T, int>> GetEnumerator() => _counter.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _counter.GetEnumerator();

        public void Clear() => _counter.Clear();

        private int Get(in T item) => _counter.GetValueOrDefault(item, 0);

        private void Set(in T item, int value)
        {
            if (value > 0)
                _counter[item] = value;
            else
                _counter.Remove(item);
        }
    }
}
