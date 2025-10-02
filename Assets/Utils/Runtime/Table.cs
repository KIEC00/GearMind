using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Utils.Runtime
{
    public interface IReadOnlyTable<T> : IEnumerable<T>
    {
        public T this[int x, int y] { get; }
        public (int width, int height) Size { get; }
    }

    public class Table<T> : IReadOnlyTable<T>
    {
        public T this[int x, int y]
        {
            get => _cells[x, y];
            set { _cells[x, y] = value; }
        }
        public (int width, int height) Size => new(_cells.GetLength(0), _cells.GetLength(1));
        public IReadOnlyTable<T> AsReadOnly => this;

        private readonly T[,] _cells;

        public Table(int width, int height) => _cells = new T[width, height];

        public Table(T[,] cells)
        {
            _cells = new T[cells.GetLength(0), cells.GetLength(1)];
            Array.Copy(cells, _cells, _cells.Length);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var cell in _cells)
                yield return cell;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
