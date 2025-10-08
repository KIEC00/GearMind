using System.Collections.Generic;
using System.Linq;
using Assets.GearMind.Grid.Components;
using UnityEngine;

namespace Assets.GearMind.Grid
{
    public class GridItem
    {
        public IReadOnlyList<Cell> Cells => _cells;
        public Vector2Int Position { get; }
        public IGridItemComponent Component { get; }

        private readonly Cell[] _cells;

        public GridItem(IEnumerable<Cell> cells, Vector2Int position, IGridItemComponent component)
        {
            _cells = cells.ToArray();
            Position = position;
            Component = component;
        }
    }
}
