using Assets.GearMind.Instruments;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
public class RailPlatform : MonoBehaviour, IDragHandler, IEndDragHandler, IGameplayObject
{
    [SerializeField, OnValueChanged(nameof(UpdateWidth)), Min(1)]
    private float _railLength = 3;

    [SerializeField, OnValueChanged(nameof(UpdateWidth)), Min(1)]
    private float _platformLength = 1;

#if UNITY_EDITOR
    [SerializeField, OnValueChanged(nameof(UpdateInitialPosition))]
    private float _initialPosition = 0;
#endif

    [Space]
    [SerializeField, Required]
    private Transform _platformTransform;

    [SerializeField, Required]
    private Rigidbody2D _platformRigidbody;

    [SerializeField, Required]
    private Transform _railTransform;

    private float _editorPositionX;
    private readonly RaycastHit2D[] _castResult = new RaycastHit2D[1];

    private void Awake()
    {
        _platformTransform = _platformRigidbody.transform;
        _editorPositionX = _platformTransform.localPosition.x;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var newLocalPositionX = ClampPosition(GetTargetLocalPosition(eventData));
        var newLocalPosition = GetPlatformLocalPosition(newLocalPositionX);
        var newWorldPos = (Vector2)_platformTransform.parent.TransformPoint(newLocalPosition);

        var currentPos = _platformRigidbody.position;
        var delta = newWorldPos - currentPos;
        var direction = delta.normalized;
        var hitCount = _platformRigidbody.Cast(direction, _castResult, delta.magnitude);

        if (hitCount > 0)
        {
            var hit = _castResult[0];
            var hitRigidbody = hit.collider.attachedRigidbody;
            if (hitRigidbody && hitRigidbody.bodyType == RigidbodyType2D.Dynamic)
            {
                _platformRigidbody.MovePosition(newWorldPos);
                return;
            }

            var dot = Vector2.Dot(direction, hit.normal);
            if (dot >= 0f)
            {
                _platformRigidbody.MovePosition(newWorldPos);
                return;
            }

            var allowedDist = hit.distance;
            if (allowedDist <= 0f)
                return;
            var allowedWorldPos = currentPos + delta.normalized * allowedDist;
            _platformRigidbody.MovePosition(allowedWorldPos);
            return;
        }

        _platformRigidbody.MovePosition(newWorldPos);
    }

    public void OnEndDrag(PointerEventData eventData) =>
        _platformRigidbody.linearVelocity = Vector2.zero;

    private float GetTargetLocalPosition(PointerEventData eventData)
    {
        var camera = eventData.pressEventCamera;
        var pressZ = eventData.pointerPressRaycast.worldPosition.z;
        var cameraZDistance = pressZ - camera.transform.position.z;
        var cameraPosition = eventData.position;
        var platformTransform = _platformTransform;
        var worldPosition = (Vector2)
            camera.ScreenToWorldPoint(cameraPosition.WithZ(cameraZDistance));
        var localPosition = platformTransform.InverseTransformPoint(worldPosition);
        return localPosition.x + platformTransform.localPosition.x;
    }

    public void EnterEditMode() =>
        _platformTransform.localPosition = GetPlatformLocalPosition(_editorPositionX);

    public void EnterPlayMode() => _editorPositionX = _platformTransform.localPosition.x;

    private Vector3 GetPlatformLocalPosition(float localX)
    {
        var position = _platformTransform.localPosition;
        position.x = localX;
        return position;
    }

    private float ClampPosition(float localX)
    {
        var railLengthHalf = (_railLength - _platformLength) / 2;
        return Mathf.Clamp(localX, -railLengthHalf, railLengthHalf);
    }

    private void UpdateWidth()
    {
#if UNITY_EDITOR
        var newPlatformScale = _platformTransform.localScale;
        newPlatformScale.x = _platformLength;
        _platformTransform.localScale = newPlatformScale;

        _railLength = Mathf.Max(_railLength, _platformLength);

        var newRailScale = _railTransform.localScale;
        newRailScale.x = _railLength - 0.01f;
        _railTransform.localScale = newRailScale;

        _platformTransform.localPosition = GetPlatformLocalPosition(
            ClampPosition(_platformTransform.localPosition.x)
        );

        UpdateInitialPosition();
#endif
    }

    private void UpdateInitialPosition()
    {
#if UNITY_EDITOR
        _initialPosition = ClampPosition(_initialPosition);
        _platformTransform.localPosition = GetPlatformLocalPosition(_initialPosition);
#endif
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!_platformRigidbody)
            _platformRigidbody = GetComponentInChildren<Rigidbody2D>();
        if (_platformRigidbody)
        {
            if (!_platformTransform)
                _platformTransform = _platformRigidbody.transform;
            _platformRigidbody.gravityScale = 0;
            _platformRigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }
#endif
}
