using System;
using System.Collections.Generic;

namespace Assets.Utils.Runtime
{
    public static class DictionaryExtensions
    {
        public static V GetOrAdd<K, V>(
            this Dictionary<K, V> dictionary,
            K key,
            V defaultValue = default
        )
        {
            if (dictionary.TryGetValue(key, out V value))
                return value;
            dictionary.Add(key, defaultValue);
            return defaultValue;
        }

        public static V GetOrAdd<K, V>(
            this Dictionary<K, V> dictionary,
            K key,
            Func<V> valueFactory
        )
        {
            if (dictionary.TryGetValue(key, out V value))
                return value;
            value = valueFactory();
            dictionary.Add(key, value);
            return value;
        }

        public static V GetOrAdd<K, V>(this Dictionary<K, V> dictionary, K key)
            where V : new()
        {
            if (dictionary.TryGetValue(key, out V value))
                return value;
            value = new V();
            dictionary.Add(key, value);
            return value;
        }
    }
}
