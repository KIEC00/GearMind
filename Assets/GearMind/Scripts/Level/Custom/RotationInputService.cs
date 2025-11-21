using System;
using Assets.GearMind.Input;
using UnityEngine.InputSystem;

namespace Assets.GearMind.Custom.Input
{
    public class RotationInputService : IRotationInputService, IDisposable
    {
        public bool Enabled
        {
            get => Actions.enabled;
            set => SetEnabled(value);
        }

        public Direction? RotationDirection => _direction;

        public event Action<Direction> RotationStart;
        public event Action RotationStop;
        public event Action EscPressed;

        private readonly InputActions _inputAction = new();
        private InputActions.RotationGameplayActions Actions => _inputAction.RotationGameplay;
        private Direction? _direction = null;

        private bool _rotateClockwisePressed;
        private bool _rotateCounterclockwisePressed;

        public RotationInputService()
        {
            Actions.RotateClockwise.started += HadleRotateClockwiseStart;
            Actions.RotateClockwise.canceled += HadleRotateClockwiseStop;
            Actions.RotateCounterclockwise.started += HadleRotateCounterclockwiseStart;
            Actions.RotateCounterclockwise.canceled += HadleRotateCounterclockwiseStop;
            Actions.Esc.started += HandleEscPressed;
        }

        private void HadleRotateClockwiseStart(InputAction.CallbackContext context)
        {
            _rotateClockwisePressed = true;
            HandleStartRotate(Direction.Clockwise);
        }

        private void HadleRotateClockwiseStop(InputAction.CallbackContext context)
        {
            _rotateClockwisePressed = false;
            HandleEndRotate(Direction.Clockwise);
        }

        private void HadleRotateCounterclockwiseStart(InputAction.CallbackContext context)
        {
            _rotateCounterclockwisePressed = true;
            HandleStartRotate(Direction.Counterclockwise);
        }

        private void HadleRotateCounterclockwiseStop(InputAction.CallbackContext context)
        {
            _rotateCounterclockwisePressed = false;
            HandleEndRotate(Direction.Counterclockwise);
        }

        private void HandleStartRotate(Direction direction)
        {
            if (_direction == direction)
                return;
            if (_direction.HasValue)
                RotationStop?.Invoke();
            _direction = direction;
            RotationStart?.Invoke(_direction.Value);
        }

        private void HandleEndRotate(Direction direction)
        {
            if (_direction != direction)
                return;

            RotationStop?.Invoke();

            if (direction == Direction.Clockwise && _rotateCounterclockwisePressed)
                _direction = Direction.Counterclockwise;
            else if (direction == Direction.Counterclockwise && _rotateClockwisePressed)
                _direction = Direction.Clockwise;
            else
                _direction = null;

            if (_direction.HasValue)
                RotationStart?.Invoke(_direction.Value);
        }

        private void HandleEscPressed(InputAction.CallbackContext context) => EscPressed?.Invoke();

        public void Enable() => Actions.Enable();

        public void Disable() => _inputAction.Disable();

        public void SetEnabled(bool value)
        {
            if (value)
                Enable();
            else
                Disable();
        }

        public void Dispose() => _inputAction.Dispose();
    }
}
