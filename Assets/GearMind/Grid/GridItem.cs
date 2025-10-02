using System.Collections.Generic;
using System.Linq;

namespace Assets.GearMind.Grid
{
    public class GridItem
    {
        public IReadOnlyList<Cell> Cells => _cells;
        public object Payload { get; }

        private readonly Cell[] _cells;

        public GridItem(IEnumerable<Cell> cells, object payload)
        {
            _cells = cells.ToArray();
            Payload = payload;
        }
    }
}
