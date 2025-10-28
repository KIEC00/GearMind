using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.GearMind.Input
{
    public class InputService : IInputService, IDisposable
    {
        const float _dragThreshold = 5f;

        public bool Enabled
        {
            get => Actions.enabled;
            set => SetEnabled(value);
        }

        public bool IsPointerDown => Actions.MouseLeftButton.IsPressed();

        public event Action<Vector2> OnPointerPressed;
        public event Action<Vector2> OnPointerReleased;
        public event Action<Vector2> OnPointerClick;

        public bool IsPointerAltDown => Actions.MouseRightButton.IsPressed();
        public event Action<Vector2> OnPointerAltPressed;
        public event Action<Vector2> OnPointerAltReleased;
        public event Action<Vector2> OnPointerAltClick;

        public Vector2 PointerPosition => Actions.PointerPosition.ReadValue<Vector2>();
        public Vector2 PointerDelta => Actions.PointerMove.ReadValue<Vector2>();
        public event Action<PointerMove> OnPointerMove;

        public bool IsDraging { get; private set; }
        public event Action<Vector2> OnDragStart;
        public event Action<PointerMove> OnDrag;
        public event Action<Vector2> OnDragEnd;

        public event Action OnEscPressed;

        private readonly InputActions _inputAction = new();
        private InputActions.GamePlayActions Actions => _inputAction.GamePlay;

        private Vector2 _pointerPressedPosition = Vector2.zero;

        public InputService()
        {
            Actions.MouseLeftButton.started += HandlePointerPressed;
            Actions.MouseLeftButton.canceled += HandlePointerReliased;
            Actions.MouseRightButton.started += HandlePointerAltPressed;
            Actions.MouseRightButton.canceled += HandlePointerAltReliased;
            Actions.PointerPosition.performed += HandlePointerPosition;
            Actions.EscPressed.started += HandleEscPressed;
        }

        public void Enable() => Actions.Enable();

        public void Disable() => _inputAction.Disable();

        public void SetEnabled(bool value)
        {
            if (value)
                Enable();
            else
                Disable();
        }

        public void ForceStartDrag()
        {
            var position = PointerPosition;
            HandleStartDrag(position);
            if (!IsPointerDown)
                HandleEndDrag(position);
        }

        private void HandlePointerPressed(InputAction.CallbackContext ctx)
        {
            var currentPosition = PointerPosition;
            OnPointerPressed?.Invoke(currentPosition);
            _pointerPressedPosition = currentPosition;
        }

        private void HandlePointerReliased(InputAction.CallbackContext ctx)
        {
            var currentPosition = PointerPosition;
            OnPointerReleased?.Invoke(currentPosition);
            if (IsDraging)
                HandleEndDrag(currentPosition);
            else
                OnPointerClick?.Invoke(currentPosition);
        }

        private void HandlePointerAltPressed(InputAction.CallbackContext ctx) =>
            OnPointerAltPressed?.Invoke(PointerPosition);

        private void HandlePointerAltReliased(InputAction.CallbackContext ctx)
        {
            var currentPosition = PointerPosition;
            OnPointerAltReleased?.Invoke(currentPosition);
            if (IsDragingTrashold(_pointerPressedPosition, currentPosition))
                return;
            OnPointerAltClick?.Invoke(currentPosition);
        }

        private void HandlePointerPosition(InputAction.CallbackContext ctx)
        {
            var move = new PointerMove(PointerPosition, PointerDelta);
            OnPointerMove?.Invoke(move);
            if (IsDraging)
                OnDrag?.Invoke(move);
            else if (IsPointerDown && IsDragingTrashold(_pointerPressedPosition, move.Current))
                HandleStartDrag(move.Current);
        }

        private void HandleStartDrag(Vector2 position)
        {
            IsDraging = true;
            OnDragStart?.Invoke(position);
            OnDrag?.Invoke(new PointerMove(position, position - _pointerPressedPosition));
        }

        private void HandleEndDrag(Vector2 position)
        {
            IsDraging = false;
            OnDragEnd?.Invoke(position);
        }

        private void HandleEscPressed(InputAction.CallbackContext context) =>
            OnEscPressed?.Invoke();

        private bool IsDragingTrashold(Vector2 start, Vector2 end) =>
            Vector2.Distance(start, end) > _dragThreshold;

        public void Dispose() => _inputAction.Dispose();
    }
}
