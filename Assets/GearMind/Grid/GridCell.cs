using System.Collections.Generic;

namespace Assets.GearMind.Grid
{
    public readonly struct GridItemRecord
    {
        public readonly GridItem Item;
        public readonly Cell Cell;

        public GridItemRecord(GridItem item, Cell cell)
        {
            Item = item;
            Cell = cell;
        }
    }

    public interface IReadOnlyGridCell
    {
        IReadOnlyCollection<GridItemRecord> Records { get; }
    }

    public class GridCell : IReadOnlyGridCell
    {
        public IReadOnlyCollection<GridItemRecord> Records => _records;

        private readonly List<GridItemRecord> _records = new();

        public void Add(GridItem item, Cell cell) => _records.Add(new(item, cell));

        public void Remove(GridItem item, Cell cell) => _records.Remove(new(item, cell));
    }
}
