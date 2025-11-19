using System;
using System.Collections;
using Assets.GearMind.Custom.Input;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Custom.Level
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RotationTargetComponent : MonoBehaviour, IRotationTarget
    {
        [Header("Config")]
        [SerializeField]
        private float _rotationSpeed = 90f;

        [SerializeField]
        private float _acceleration = 180f;

        [SerializeField]
        private float _deceleration = 180f;

        [Space]
        [SerializeField, Required]
        private Rigidbody2D _rigidbody;

        [Header("Debug")]
        [ShowInInspector]
        private Direction _currentDirection;

        [ShowInInspector]
        private float _currentAngularVelocity;

        private Coroutine _rotationRoutine;

        private readonly YieldInstruction _waitInstruction = new WaitForFixedUpdate();

        public void StartRotation(Direction direction)
        {
            if (_currentDirection == direction)
                return;

            if (_rotationRoutine != null)
                StopCoroutine(_rotationRoutine);

            _currentDirection = direction;
            _rotationRoutine = StartCoroutine(RotationRoutine());
        }

        public void StopRotation()
        {
            if (_rotationRoutine != null)
                StopCoroutine(_rotationRoutine);

            if (_currentDirection != 0)
                _rotationRoutine = StartCoroutine(StopRotationRoutine());

            _currentDirection = 0;
        }

        public void ImmidiatlyStop()
        {
            if (_rotationRoutine != null)
            {
                StopCoroutine(_rotationRoutine);
                _rotationRoutine = null;
            }
            _currentAngularVelocity = 0f;
        }

        private IEnumerator RotationRoutine()
        {
            while (true)
            {
                var directionMultiplier = (int)_currentDirection;
                var currentAccel =
                    (_currentAngularVelocity * directionMultiplier < 0)
                        ? _acceleration + _deceleration
                        : _acceleration;

                var deltaTime = Time.deltaTime;
                _currentAngularVelocity += directionMultiplier * currentAccel * deltaTime;

                if (Mathf.Abs(_currentAngularVelocity) > _rotationSpeed)
                    _currentAngularVelocity = directionMultiplier * _rotationSpeed;

                _rigidbody.MoveRotation(_rigidbody.rotation + _currentAngularVelocity * deltaTime);

                yield return _waitInstruction;
            }
        }

        private IEnumerator StopRotationRoutine()
        {
            while (Mathf.Abs(_currentAngularVelocity) > 0.1f)
            {
                var stopDirection = -Math.Sign(_currentAngularVelocity);
                var deltaTime = Time.deltaTime;

                var previousVelocity = _currentAngularVelocity;
                _currentAngularVelocity += stopDirection * _deceleration * deltaTime;

                if (previousVelocity * _currentAngularVelocity <= 0)
                    break;

                _rigidbody.MoveRotation(_rigidbody.rotation + _currentAngularVelocity * deltaTime);

                yield return _waitInstruction;
            }

            _currentAngularVelocity = 0f;
            _rotationRoutine = null;
        }

        void OnValidate()
        {
            if (!_rigidbody)
                _rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}
