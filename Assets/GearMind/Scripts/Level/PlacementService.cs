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
        public Action<GameObject> DestroyRequest;

        private readonly IInputService _input;
        private readonly ICameraProvider _cameraProvider;
        private readonly IScreenRaycaster2D _raycaster;
        private readonly GridComponent _grid;
        private bool _enabled;

        private DraggingData _draggingData = null;

        public PlacementService(
            IInputService input,
            ICameraProvider cameraProvider,
            IScreenRaycaster2D raycaster,
            GridComponent grid
        )
        {
            _input = input;
            _cameraProvider = cameraProvider;
            _raycaster = raycaster;
            _grid = grid;
            _input.Disable();
        }

        public void StartDragObject(GameObject gameObject)
        {
            if (!SetDraggingObject(gameObject))
            {
                DestroyRequest?.Invoke(gameObject);
                Debug.LogWarning($"Failed to set dragging object {gameObject.name}");
                return;
            }
            var position = _grid.ScreenToPlane(_input.PointerPosition, _cameraProvider.Current);
            gameObject.transform.position = position ?? Vector3.zero;
        }

        private bool SetDraggingObject(GameObject gameObject)
        {
            if (
                gameObject
                && gameObject.TryGetComponent<IDragAndDropTarget>(out var draggable)
                && draggable.IsDragable
            )
            {
                _draggingData = new()
                {
                    DragTarget = draggable,
                    GameplayObject = gameObject.GetComponent<IGameplayObject>(),
                };
                return true;
            }
            _draggingData = null;
            return false;
        }

        private void HandleStartDrag(Vector2 position)
        {
            var collider = _raycaster.RaycastPhysics2DStopAtUI(position);
            if (!SetDraggingObject(collider ? collider.gameObject : null))
                return;
            var hitPosition = _raycaster.ScreenToWorldPoint2D(position);
            _draggingData.DragOffset = (Vector2)collider.transform.position - hitPosition;
            _draggingData.DragTarget.OnDragStart();
        }

        private void HandleDrag(PointerMove move)
        {
            if (_draggingData == null)
                return;
            var planePosition = _grid.ScreenToPlane(move.Current, _cameraProvider.Current);
            var position = _grid.SnapToGrid(
                (planePosition ?? Vector3.zero) + _draggingData.DragOffset
            );
            _draggingData.DragTarget.OnDrag(position);
            var canPlaceObject = _draggingData.DragTarget.ValidatePlacement(out var dependsOn);
            if (canPlaceObject != _draggingData.CanPlace)
            {
                _draggingData.CanPlace = canPlaceObject;
                _draggingData.DragTarget.SetError(!canPlaceObject);
            }
        }

        private void HandleDragEnd(Vector2 screenPosition)
        {
            if (_draggingData == null)
                return;
            var planePosition = _grid.ScreenToPlane(screenPosition, _cameraProvider.Current);
            var position = _grid.SnapToGrid(
                (planePosition ?? Vector3.zero) + _draggingData.DragOffset
            );
            _draggingData.DragTarget.OnDrag(position);
            var canPlaceObject = _draggingData.DragTarget.ValidatePlacement(out var dependsOn);
            if (canPlaceObject)
            {
                _draggingData.DragTarget.OnDragEnd();
                _draggingData.GameplayObject.EnterEditMode();
            }
            else
            {
                var gameObject = ((MonoBehaviour)_draggingData.DragTarget).gameObject;
                DestroyRequest?.Invoke(gameObject);
            }
            _draggingData = null;
        }

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
            if (_enabled)
                return;
            _enabled = true;

            _input.Disable();
        }

        public void Dispose() => Disable();

        private class DraggingData
        {
            public IDragAndDropTarget DragTarget;
            public IGameplayObject GameplayObject;
            public Vector3 DragOffset;
            public bool CanPlace;
        }
    }
}
