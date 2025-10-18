using Assets.GearMind.Grid.Components;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.GearMind.Grid.Tests
{
    [ExecuteAlways]
    public class GridController : MonoBehaviour
    {
        [SerializeField, Required]
        private GridComponent _grid;

        [SerializeField]
        private AbstractGridObject[] _objects;

        [SerializeField]
        private GridCanvas _gridCanvas;

        private AbstractGridObject _selectedPrefab;
        private AbstractGridObject _flyingObject;
        private bool _isDragging = false;

        private void Awake() => SubscribeOnGridChangeOrInit();

        private void OnDestroy() => UnsubscribeOnGridChangeOrInit();

        private void SubscribeOnGridChangeOrInit()
        {
            if (!_grid)
                return;
            _grid.OnGridChangedOrInit -= HandleGridChangeOrInit;
            _grid.OnGridChangedOrInit += HandleGridChangeOrInit;
        }

        private void UnsubscribeOnGridChangeOrInit()
        {
            if (!_grid)
                return;
            _grid.OnGridChangedOrInit -= HandleGridChangeOrInit;
        }

        private void HandleGridChangeOrInit()
        {
            if (_gridCanvas)
                _gridCanvas.Init(_grid);
        }

        // Поиск префабов
        public void StartPlacingObject(AbstractGridObject prefab)
        {
            CancelPlacing();
            _selectedPrefab = prefab;
            _flyingObject = Instantiate(_selectedPrefab);
            PrepareFlying(_flyingObject);
        }

        public void StartPlacingObjectByIndex(int index)
        {
            if (index >= 0 && index < _objects.Length)
            {
                StartPlacingObject(_objects[index]);
            }
        }

        // Перемещение/размещение
        private void UpdateFlyingObject()
        {
            if (!_flyingObject)
                return;

            var mouse = Mouse.current.position.ReadValue();
            var mousePlane = _grid.ScreenToPlane(mouse, Camera.main);

            if (!mousePlane.HasValue)
            {
                _flyingObject.gameObject.SetActive(false);
                return;
            }

            _flyingObject.gameObject.SetActive(true);

            _flyingObject.transform.position = mousePlane.Value;

            var cellPos = _grid.ScreenToCell(mouse, Camera.main);
            var canPlace = cellPos.HasValue && _grid.CanAddItem(_flyingObject, cellPos.Value);
            if (cellPos.HasValue)
            {
                _gridCanvas.ShowCursor(
                    cellPos.Value,
                    _flyingObject.Cells,
                    canPlace ? CursorType.Ok : CursorType.Error
                );
            }
            else
            {
                _gridCanvas.HideCursor();
            }

            if (!_isDragging && canPlace && Mouse.current.leftButton.wasPressedThisFrame)
                PlaceFlyingObject(cellPos.Value);
        }

        private void PlaceFlyingObject(Vector2Int cellPos)
        {
            if (!_flyingObject)
                return;

            AddItem(_flyingObject, cellPos);
            PrepareGrid(_flyingObject);

            _flyingObject = null;
            _selectedPrefab = null;
        }

        private void PrepareFlying(AbstractGridObject obj)
        {
            var physics = obj.GetComponentInChildren<Rigidbody2D>();
            if (physics)
                physics.simulated = false;

            var colliders = obj.GetComponentsInChildren<Collider2D>();
            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }
        }

        private void PrepareGrid(AbstractGridObject obj)
        {
            var physics = obj.GetComponentInChildren<Rigidbody2D>();
            if (physics)
                physics.simulated = true;

            var colliders = obj.GetComponentsInChildren<Collider2D>();
            foreach (var collider in colliders)
            {
                collider.enabled = true;
            }

            foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
                renderer.material.color = Color.white;
        }

        private bool AddItem(AbstractGridObject item, Vector2Int cellPosition)
        {
            if (!_grid.CanAddItem(item, cellPosition))
                return false;

            _grid.AddItem(item, cellPosition);

            var position = _grid.CellToWorld(cellPosition);
            item.transform.position = position;
            return true;
        }

        private void HandleCancel()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
                CancelPlacing();
        }

        public void CancelPlacing()
        {
            if (_flyingObject != null)
                Destroy(_flyingObject.gameObject);

            _flyingObject = null;
            _selectedPrefab = null;
            _isDragging = false;
        }

        // Drag & Drop
        private GridItem GetSolidItemAt(Vector2 screenPosition)
        {
            var cellPos = _grid.ScreenToCell(screenPosition, Camera.main);
            if (!cellPos.HasValue)
                return null;
            return _grid.Cells[cellPos.Value].FirstOrNull(rec => rec.Cell.IsSolid)?.Item;
        }

        private void HandleStartDrag()
        {
            if (!Mouse.current.leftButton.wasPressedThisFrame || _flyingObject != null)
                return;

            var mouse = Mouse.current.position.ReadValue();
            var cellItem = GetSolidItemAt(mouse);

            if (cellItem != null)
            {
                _isDragging = true;

                var removedObjects = _grid.RemoveItemRecursive(cellItem.Component);
                if (removedObjects == null)
                    return;

                foreach (var removedItem in removedObjects)
                    if (removedItem != cellItem)
                        Destroy(((AbstractGridObject)removedItem.Component).gameObject);

                _flyingObject = (AbstractGridObject)cellItem.Component;
                PrepareFlying(_flyingObject);
            }
        }

        private void HandleEndDrag()
        {
            if (!_isDragging || !Mouse.current.leftButton.wasReleasedThisFrame)
                return;

            _isDragging = false;

            if (!_flyingObject)
                return;

            var mouse = Mouse.current.position.ReadValue();
            var cellPos = _grid.ScreenToCell(mouse, Camera.main);

            if (cellPos.HasValue && AddItem(_flyingObject, cellPos.Value))
            {
                PrepareGrid(_flyingObject);
            }
            else
            {
                Destroy(_flyingObject.gameObject);
            }

            _flyingObject = null;
        }

        private void Update()
        {
            HandleCancel();
            HandleStartDrag();
            UpdateFlyingObject();
            HandleEndDrag();
        }

        void OnValidate()
        {
            if (!_grid)
                _grid = GetComponent<GridComponent>();
            if (_grid)
                SubscribeOnGridChangeOrInit();
        }
    }
}
