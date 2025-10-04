using System.Collections.Generic;
using Assets.Utils.Runtime;
using UnityEngine;

namespace Assets.GearMind.Grid
{
    public partial class Grid
    {
        public IReadOnlyGridCell this[int x, int y] => _table[x, y];
        public IReadOnlyGridCell this[Vector2Int position] => _table[position.x, position.y];

        private readonly Table<GridCell> _table;

        public Grid(int width, int height)
        {
            _table = new Table<GridCell>(width, height).Fill(() => new GridCell());
        }

        public bool AddItem(GridItem item, Vector2Int position)
        {
            if (!CanAddItem(item, position))
                return false;
            AddItemUnsave(item, position);
            return true;
        }

        public bool AddItem(
            GridItem item,
            Vector2Int position,
            out IEnumerable<GridItem> attachedTo
        )
        {
            if (!CanAddItem(item, position, out attachedTo))
                return false;
            AddItemUnsave(item, position);
            return true;
        }

        public IReadOnlyCollection<GridItem> RemoveItemsRecursive(GridItem targetItem)
        {
            var removeTargets = GetRecursiveRemoveTargets(targetItem);
            foreach (var item in removeTargets)
                RemoveItemUnsave(item);
            return removeTargets;
        }

        private void AddItemUnsave(GridItem item, Vector2Int position)
        {
            foreach (var (i, cell) in item.Cells.Enumerate())
            {
                var gridPosition = position + cell.Position;
                var gridCell = _table[gridPosition.x, gridPosition.y];
                gridCell.Add(new(item, i));
            }
        }

        private void RemoveItemUnsave(GridItem item)
        {
            var position = item.Position;
            foreach (var (i, cell) in item.Cells.Enumerate())
            {
                var gridPosition = position + cell.Position;
                var gridCell = _table[gridPosition.x, gridPosition.y];
                gridCell.Remove(new(item, i));
            }
        }
    }
}
