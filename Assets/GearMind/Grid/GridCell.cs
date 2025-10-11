using System.Collections;
using System.Collections.Generic;

namespace Assets.GearMind.Grid
{
    public readonly struct CellRecord
    {
        public Cell Cell => Item.Cells[CellIndex];
        public GridItem Item { get; }
        public int CellIndex { get; }

        public CellRecord(GridItem item, int cellIndex)
        {
            Item = item;
            CellIndex = cellIndex;
        }
    }

    public interface IReadOnlyGridCell : IReadOnlyList<CellRecord> { }

    public class GridCell : IReadOnlyGridCell
    {
        public CellRecord this[int index]
        {
            get => _records[index];
            set => _records[index] = value;
        }

        public int Count => _records.Count;

        private readonly List<CellRecord> _records = new();

        public void Add(CellRecord record) => _records.Add(record);

        public void Remove(CellRecord record) => _records.Remove(record);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<CellRecord> GetEnumerator() => _records.GetEnumerator();
    }
}
