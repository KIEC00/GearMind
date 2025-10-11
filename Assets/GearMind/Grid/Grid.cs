using System.Linq;
using Assets.Utils.Runtime;
using UnityEngine;

namespace Assets.GearMind.Grid
{
    public class Grid
    {
        public IReadOnlyGridCell this[int x, int y] => _table[x, y];
        public IReadOnlyGridCell this[Vector2Int position] => _table[position.x, position.y];

        private readonly Table<GridCell> _table;

        public Grid(int width, int height) =>
            _table = new Table<GridCell>(width, height).Fill(() => new GridCell());

        public bool IsPositionInBounds(Vector2Int position) =>
            position.x >= 0
            && position.x < _table.Size.width
            && position.y >= 0
            && position.y < _table.Size.height;

        public bool IsItemInBounds(GridItem item, Vector2Int position) =>
            item.Cells.All((cell) => IsPositionInBounds(position + cell.Offset));

        public bool AddItem(GridItem item, Vector2Int position)
        {
            if (!IsItemInBounds(item, position))
                return false;
            AddItemUnsave(item, position);
            return true;
        }

        public void AddItemUnsave(GridItem item, Vector2Int position)
        {
            foreach (var (i, cell) in item.Cells.Enumerate())
            {
                var gridPosition = position + cell.Offset;
                var gridCell = _table[gridPosition.x, gridPosition.y];
                gridCell.Add(new(item, i));
            }
        }

        public void RemoveItem(GridItem item)
        {
            var position = item.Position;
            foreach (var (i, cell) in item.Cells.Enumerate())
            {
                var gridPosition = position + cell.Offset;
                var gridCell = _table[gridPosition.x, gridPosition.y];
                gridCell.Remove(new(item, i));
            }
        }
    }
}
