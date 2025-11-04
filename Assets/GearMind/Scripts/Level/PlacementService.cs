using System;
using Assets.GearMind.Common;
using Assets.GearMind.Grid;
using Assets.GearMind.Input;
using Assets.GearMind.Objects;
using UnityEngine;

namespace Assets.GearMind.Level
{
    public class PlacementService : IDisposable
    {
        public bool IsDragging => _draggingData;
        public Action OnDragStart;
        public Action OnDragEnd;

        private readonly IInputService _input;
        private readonly ICameraProvider _cameraProvider;
        private readonly IScreenRaycaster2D _raycaster;
        private readonly GridComponent _grid;
        private readonly IObjectService _objectService;
        private readonly Transform _objectsParent;
        private readonly LayerMask _layerMask;
        private bool _enabled;

        private DraggingData _draggingData = new();

        // private readonly Dictionary<IDragAndDropTarget, HashSet<IDragAndDropTarget>> _dependencies;
        // TODO: Implement dependencies

        public PlacementService(
            IInputService input,
            ICameraProvider cameraProvider,
            IScreenRaycaster2D raycaster,
            GridComponent grid,
            IObjectService objectService,
            Transform objectsParent,
            LayerMask layerMask
        )
        {
            _input = input;
            _cameraProvider = cameraProvider;
            _raycaster = raycaster;
            _grid = grid;
            _objectService = objectService;
            _objectsParent = objectsParent;
            _layerMask = layerMask;
            _input.Disable();
        }

        public void InstantiateAndStartDragObject(GameObject prefab)
        {
            if (!_enabled)
            {
                Debug.LogWarning("PlacementService is not enabled");
                return;
            }
            var gameObject = _objectService.InstantiateObject(prefab);
            if (!SetDraggingObject(gameObject, isFromPrefab: true))
            {
                DestroyGameobject(gameObject);
                Debug.LogWarning($"Failed to set dragging object {gameObject.name}");
                return;
            }
            gameObject.transform.SetParent(_objectsParent, true);
            _draggingData.GameplayObject?.EnterEditMode();
            var position = _grid.ScreenToPlane(_input.PointerPosition, _cameraProvider.Current);
            gameObject.transform.position = position ?? Vector3.zero;
            _draggingData.DragOffset = Vector2.zero;
            _input.ForceStartDrag();
        }

        private bool SetDraggingObject(GameObject gameObject, bool isFromPrefab = false)
        {
            var draggable = GetDragable(gameObject);
            if (draggable == null || (draggable.IsDragable == false && !isFromPrefab))
            {
                _draggingData.DragTarget = null;
                return false;
            }
            if (isFromPrefab)
                draggable.IsDragable = true;
            _draggingData = new() { DragTarget = draggable };
            if (gameObject.TryGetComponent<IGameplayObject>(out var gameplayObject))
                _draggingData.GameplayObject = gameplayObject;
            return true;
        }

        private void HandleStartDrag(Vector2 position)
        {
            if (!_draggingData && !RaycastOnStartDrag(position))
                return;
            _draggingData.DragTarget.OnDragStart();
            OnDragStart?.Invoke();
        }

        private void HandleDrag(PointerMove move)
        {
            if (!_draggingData)
                return;
            GetPositions(move.Current, out var planePositionWithOffset, out var gridPosition);
            var canPlaceObject =
                gridPosition.HasValue && _draggingData.DragTarget.ValidatePlacement();
            _draggingData.DragTarget.SetError(!canPlaceObject);
            _draggingData.DragTarget.OnDrag(gridPosition ?? planePositionWithOffset);
        }

        private void HandleDragEnd(Vector2 screenPosition)
        {
            if (!_draggingData)
                return;
            OnDragEnd?.Invoke();
            GetPositions(screenPosition, out var _, out var gridPosition);
            if (gridPosition.HasValue && _draggingData.DragTarget.ValidatePlacement())
                HandlePlace(_draggingData);
            else
                HandleDestroy(_draggingData.DragTarget);
            _draggingData.DragTarget = null;
        }

        private void HandlePlace(DraggingData draggingData)
        {
            draggingData.DragTarget.OnDragEnd();
            draggingData.GameplayObject?.EnterEditMode();
        }

        private void HandleDestroy(IDragAndDropTarget target) =>
            DestroyGameobject(((MonoBehaviour)target).gameObject);

        private void DestroyGameobject(GameObject gameObject) =>
            _objectService.DestroyObject(gameObject);

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _input.OnDragStart += HandleStartDrag;
            _input.OnDrag += HandleDrag;
            _input.OnDragEnd += HandleDragEnd;

            _input.Enable();
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _input.Disable();
        }

        public void Dispose() => Disable();

        private static IDragAndDropTarget GetDragable(GameObject gameObject) =>
            (gameObject && gameObject.TryGetComponent<IDragAndDropTarget>(out var draggable))
                ? draggable
                : null;

        private void GetPositions(
            Vector2 screenPosition,
            out Vector3 planePositionWithOffset,
            out Vector3? gridPosition
        )
        {
            var camera = _cameraProvider.Current;
            var planePosition = _grid.ScreenToPlane(screenPosition, camera) ?? Vector3.zero;
            planePositionWithOffset = planePosition + _draggingData.DragOffset;
            gridPosition = _grid.SnapToGrid(planePositionWithOffset);
        }

        private bool RaycastOnStartDrag(Vector2 position)
        {
            var collider = _raycaster.RaycastPhysics2DStopAtUI(position, _layerMask);
            if (!SetDraggingObject(collider ? collider.gameObject : null))
                return false;
            var hitPosition = _raycaster.ScreenToWorldPoint2D(position);
            _draggingData.DragOffset = (Vector2)collider.transform.position - hitPosition;
            return true;
        }

        private struct DraggingData
        {
            public IDragAndDropTarget DragTarget;
            public IGameplayObject GameplayObject;
            public Vector3 DragOffset;

            public static implicit operator bool(DraggingData data) => data.DragTarget != null;
        }
    }
}
