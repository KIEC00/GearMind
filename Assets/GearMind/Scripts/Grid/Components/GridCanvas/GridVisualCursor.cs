using System.Collections.Generic;
using System.Linq;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GearMind.Grid.Components
{
    class GridVisualCursor : MonoBehaviour
    {
        [SerializeField, Required]
        private Image _cursorPrefab;

        private readonly List<Image> _cursorInstances = new();

        private GridComponent _grid;

        public void Init(GridComponent grid) => _grid = grid;

        public void ShowCursor(Vector2Int position, IEnumerable<Cell> cells, CursorType type)
        {
            var cellsArray = cells.Where(cell => cell.IsSolid).ToArray();
            if (cellsArray.Length < _cursorInstances.Count)
            {
                for (var i = cellsArray.Length; i < _cursorInstances.Count; i++)
                    Destroy(_cursorInstances[i].gameObject);
                _cursorInstances.RemoveRange(
                    cellsArray.Length,
                    _cursorInstances.Count - cellsArray.Length
                );
            }
            else if (cellsArray.Length > _cursorInstances.Count)
            {
                var cellSize = _grid.CellScale;
                for (var i = _cursorInstances.Count; i < cellsArray.Length; i++)
                {
                    _cursorInstances.Add(Instantiate(_cursorPrefab, transform));
                    _cursorInstances[i].rectTransform.sizeDelta = new Vector2(cellSize, cellSize);
                }
            }
            foreach (var (cell, instance) in cellsArray.Zip(_cursorInstances))
            {
                var gridPosition = position + cell.Offset;
                var worldPosition = _grid.CellToWorld(gridPosition);
                instance.rectTransform.position = worldPosition;
                instance.gameObject.SetActive(true);
                instance.color = GetColor(type);
            }
        }

        public void HideCursor()
        {
            foreach (var instance in _cursorInstances)
                instance.gameObject.SetActive(false);
        }

        private Color GetColor(CursorType type) =>
            type switch
            {
                CursorType.None => Color.white,
                CursorType.Ok => Color.green,
                CursorType.Error => Color.red,
                _ => Color.white,
            };

        private void OnDestroy()
        {
            foreach (var instance in _cursorInstances)
                Destroy(instance.gameObject);
        }
    }

    public enum CursorType
    {
        None,
        Ok,
        Error,
    }
}
