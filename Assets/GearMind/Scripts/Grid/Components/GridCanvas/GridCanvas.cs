using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GearMind.Grid.Components
{
    [RequireComponent(typeof(Canvas))]
    public class GridCanvas : MonoBehaviour
    {
        [SerializeField, Required]
        private Canvas _gridCanvas;

        [SerializeField, Required]
        private Image _gridImage;

        [SerializeField, Required]
        private GridVisualCursor _cursor;

        private GridComponent _grid;

        public void Init(GridComponent grid)
        {
            if (grid == null)
                return;
            _grid = grid;

            var bounds = _grid.WorldBounds;
            var rectTransform = _gridCanvas.GetComponent<RectTransform>();
            rectTransform.position = bounds.center;
            rectTransform.sizeDelta = new Vector2(bounds.size.x, bounds.size.y);

            _gridImage.pixelsPerUnitMultiplier = 100 / grid.CellScale;

            _cursor.Init(grid);
        }

        public void ShowCursor(Vector2Int position, IEnumerable<Cell> cells, CursorType type) =>
            _cursor.ShowCursor(position, cells, type);

        public void HideCursor() => _cursor.HideCursor();
    }
}
