using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Utils.Runtime
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<(int index, T value)> Enumerate<T>(
            this IEnumerable<T> items,
            int startIndex = 0
        )
        {
            using var sequenceEnum = items.GetEnumerator();
            while (sequenceEnum.MoveNext())
            {
                yield return (startIndex, sequenceEnum.Current);
                ++startIndex;
            }
        }

        public static TItem MinByKey<TItem, TKey>(
            this IEnumerable<TItem> items,
            Func<TItem, TKey> keySelector
        )
        {
            using var enumerator = items.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException("Collection is empty.");
            return MinByKeyInternal(enumerator, keySelector);
        }

        public static TItem MinByKey<TItem, TKey>(
            this IEnumerable<TItem> items,
            Func<TItem, TKey> keySelector,
            TItem defaultValue
        )
        {
            using var enumerator = items.GetEnumerator();
            if (!enumerator.MoveNext())
                return defaultValue;
            return MinByKeyInternal(enumerator, keySelector);
        }

        public static (T first, IEnumerable<T> other) Shift<T>(this IEnumerable<T> items)
        {
            using var enumerator = items.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException("Collection is empty.");
            return (enumerator.Current, enumerator.ToIEnumerable());
        }

        public static IEnumerable<(T current, T previous)> Pairwise<T>(this IEnumerable<T> items)
        {
            using var enumerator = items.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException("Collection is empty.");

            var previous = enumerator.Current;
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                yield return (current, previous);
                previous = current;
            }
        }

        public static IEnumerable<T> ToIEnumerable<T>(this IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        public static IEnumerable<(T1, T2)> Zip<T1, T2>(
            this IEnumerable<T1> items,
            IEnumerable<T2> other
        )
        {
            using var enumerator = items.GetEnumerator();
            using var otherEnumerator = other.GetEnumerator();
            while (enumerator.MoveNext() && otherEnumerator.MoveNext())
                yield return (enumerator.Current, otherEnumerator.Current);
        }

        public static IEnumerable<T> WhereNot<T>(
            this IEnumerable<T> items,
            Func<T, bool> predicate
        ) => items.Where(item => !predicate(item));

        public static T? FirstOrNull<T>(this IEnumerable<T> items, Func<T, bool> predicate)
            where T : struct
        {
            foreach (var item in items)
                if (predicate(item))
                    return item;
            return null;
        }

        public static IEnumerable<T> ConcatIfNotNull<T>(
            this IEnumerable<T> items,
            IEnumerable<T> other
        ) => other == null ? items : items.Concat(other);

        private static TItem MinByKeyInternal<TItem, TKey>(
            IEnumerator<TItem> enumerator,
            Func<TItem, TKey> keySelector
        )
        {
            var comparer = Comparer<TKey>.Default;

            TItem minItem = enumerator.Current;
            TKey minKey = keySelector(minItem);

            while (enumerator.MoveNext())
            {
                TKey key = keySelector(enumerator.Current);
                if (comparer.Compare(key, minKey) < 0)
                {
                    minItem = enumerator.Current;
                    minKey = key;
                }
            }

            return minItem;
        }
    }
}
