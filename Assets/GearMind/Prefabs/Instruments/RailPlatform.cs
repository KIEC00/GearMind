using Assets.GearMind.Instruments;
using EditorAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RailPlatform : MonoBehaviour, IDragHandler, IEndDragHandler, IGameplayObject
{
    [SerializeField, OnValueChanged(nameof(UpdateWidth)), Min(1)]
    private float _railLength = 3;

    [SerializeField, OnValueChanged(nameof(UpdateWidth)), Min(1)]
    private float _platformLength = 1;

    [SerializeField, Required]
    private Rigidbody2D _platformRigidbody;

    [SerializeField, Required]
    private Transform _railTransform;

    [SerializeField, Required]
    private RectTransform _platformRect;

    private Vector2 _editorPosition;

    private void Awake() => _editorPosition = _platformRigidbody.position;

    private void UpdateWidth()
    {
        var newPlatformScale = _platformRigidbody.transform.localScale;
        newPlatformScale.x = _platformLength;
        _platformRigidbody.transform.localScale = newPlatformScale;

        _railLength = Mathf.Max(_railLength, _platformLength);

        var newRailScale = _railTransform.localScale;
        newRailScale.x = _railLength - 0.01f;
        _railTransform.localScale = newRailScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _platformRigidbody.bodyType = RigidbodyType2D.Dynamic;

        var positionPlatform = _platformRect.localPosition.x;
        var point = eventData.position;
        var localPosition = GetTargetPosition(point);
        var localPoint = new Vector2(localPosition, 0);

        if (Mathf.Abs(positionPlatform + localPosition) >= (_railLength - _platformLength) / 2)
        {
            var t =
                (Mathf.Abs(positionPlatform) - (_railLength - _platformLength) / 2)
                / _platformLength;
            localPoint = t * -Mathf.Sign(localPosition) * Vector2.right;
        }
        var newLocal = _platformRect.TransformPoint(localPoint);

        _platformRigidbody.MovePosition(newLocal);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _platformRigidbody.linearVelocity = Vector2.zero;
        _platformRigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    private float GetTargetPosition(Vector2 point)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _platformRect,
            point,
            Camera.main,
            out var localPoint
        );
        return localPoint.x;
    }

    private void OnValidate()
    {
        if (!_platformRect)
            _platformRect = GetComponentInChildren<RectTransform>();
        if (!_platformRigidbody)
            _platformRigidbody = GetComponentInChildren<Rigidbody2D>();

        if (_platformRigidbody)
        {
            _platformRigidbody.gravityScale = 0;
            _platformRigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void EnterEditMode()
    {
        _platformRigidbody.bodyType = RigidbodyType2D.Kinematic;
        _platformRigidbody.position = _editorPosition;
    }

    public void EnterPlayMode()
    {
        _platformRigidbody.bodyType = RigidbodyType2D.Kinematic;
        _editorPosition = _platformRigidbody.position;
    }
}
