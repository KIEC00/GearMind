using System.Collections.Generic;

namespace Assets.Utils.Runtime
{
    public static class IDictionaryExtensions
    {
        public static void TryAddFrom<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IEnumerable<TKey> keys,
            TValue value = default
        )
        {
            foreach (var key in keys)
                dictionary.TryAdd(key, value);
        }

        public static void TryAddFrom<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs
        )
        {
            foreach (var (key, value) in keyValuePairs)
                dictionary.TryAdd(key, value);
        }
    }
}
