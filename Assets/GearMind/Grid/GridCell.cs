using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    public interface IReadOnlyGridCell : IReadOnlyList<CellRecord>
    {
        public CellRecord? GetSolidRecord();
        public CellFlags CombineFlags();
    }

    public class GridCell : IReadOnlyGridCell
    {
        public int Count => _records.Count;

        public CellRecord this[int index]
        {
            get => _records[index];
            set => _records[index] = value;
        }

        private readonly List<CellRecord> _records = new();

        public void Add(CellRecord record) => _records.Add(record);

        public void Remove(CellRecord record) => _records.Remove(record);

        public CellRecord? GetSolidRecord()
        {
            foreach (var record in _records)
                if (record.Cell.IsSolid)
                    return record;
            return null;
        }

        public CellFlags CombineFlags() =>
            Cell.CombineFlags(_records.Select(record => record.Cell.Flags));

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<CellRecord> GetEnumerator() => _records.GetEnumerator();
    }
}
