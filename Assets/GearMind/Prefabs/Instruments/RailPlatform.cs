using EditorAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class RailPlatform : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private float _widthRail = 3;
    [SerializeField]
    private float _widthPlatform = 1;

    [SerializeField, Required]
    private GameObject _platformObject;

    [SerializeField, Required]
    private GameObject _railObject;

    [SerializeField, Required]
    private Collider2D _platformCollider;

    [SerializeField, Required]
    private RectTransform _rectTransformPlatform;

    [SerializeField]
    private ContactFilter2D _contactFilter;

    [Button]
    private void ChangeWidth()
    {
        var platformScale = _platformObject.transform.localScale;
        _platformObject.transform.localScale = new Vector3(_widthPlatform, platformScale.y, platformScale.z);
        var railScale = _railObject.transform.localScale;
        _railObject.transform.localScale = new Vector3(_widthRail, railScale.y, railScale.z);

    }


    private void ChangePosittionPlatform(Vector3 target)
    {
        var positionPlatform = _platformObject.transform.localPosition;

        _platformCollider.attachedRigidbody.MovePosition(target);
    }


    public void OnDrag(PointerEventData eventData)
    {
        var positionPlatform = _rectTransformPlatform.localPosition.x;
        var point = eventData.position;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_platformCollider.GetComponent<RectTransform>(), point, Camera.main, out localPoint);
        localPoint = localPoint * Vector2.right;

        if (Mathf.Abs(positionPlatform + localPoint.x) >= (_widthRail - _widthPlatform) / 2)
        {
            var t = ((Mathf.Abs(positionPlatform) - (_widthRail - _widthPlatform) / 2)) / _widthPlatform;
            localPoint = t * -Mathf.Sign(localPoint.x) * Vector2.right;
        }
        var newLocal = _rectTransformPlatform.TransformPoint(localPoint);
        

        ChangePosittionPlatform(newLocal);
    }


    public void Update()
    {
        _platformCollider.attachedRigidbody.linearVelocityY = 0;
        _platformCollider.attachedRigidbody.linearVelocityX = 0;
        _rectTransformPlatform.localPosition -= _rectTransformPlatform.localPosition.y * Vector3.up;
    }
}

