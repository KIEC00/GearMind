using Assets.GearMind.Objects;
using Assets.GearMind.Test;
using EditorAttributes;
using R3;
using R3.Triggers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rope : MonoBehaviour, IGameplayObject, INotConnectedObject, IPointerClickHandler
{
    [SerializeField]
    private float _widthRope = 0.2f;

    [SerializeField, Required]
    private DistanceJoint2D _distanceJoint;

    [SerializeField, Required]
    private Collider2D _connectCollider;

    [SerializeField, Required]
    private Transform _startRopeViewTransform;

    [SerializeField, Required]
    private LineRenderer _lineRenderer;

    [SerializeField, Required]
    private Collider2D _colider;

    [SerializeField, Required]
    private Collider2D _clickCollider;

    private bool _isCanClick;

    private bool _isCanBreak;

    private Vector3 _offsetRopeView;

    

    private Vector3 _startConnectColliderPosition;

    public void EnterPlayMode()
    {
        _connectCollider.enabled = true;
        _connectCollider.attachedRigidbody.bodyType = RigidbodyType2D.Dynamic;
        _isCanClick = true;
        _clickCollider.enabled = true;
        _colider.enabled = false;

    }

    public void EnterEditMode()
    {
        _colider.enabled = true;
        _clickCollider.enabled = false;
        ReturnStartState();
        _connectCollider.gameObject.SetActive(true);
        _connectCollider.attachedRigidbody.bodyType = RigidbodyType2D.Kinematic;
        _connectCollider.enabled = false;
        _isCanClick = false;
        


    }

    public void Connect(Collider2D collider)
    {
        if (collider.isTrigger == false && collider.TryGetComponent<DefaultRigidObject>(out _) && !collider.TryGetComponent<IConnectGridObject>(out _))
        {
            var rigidbody = collider.attachedRigidbody;
            _distanceJoint.connectedBody = rigidbody;
            _connectCollider.gameObject.SetActive(false);
            _isCanBreak = true;
        }
    }

    private void RegisterMethods()
    {
        _connectCollider.OnTriggerEnter2DAsObservable().Subscribe(collider => Connect(collider)).AddTo(this);
    }

    public void Awake()
    {
        RegisterMethods();
        _startConnectColliderPosition = _connectCollider.transform.localPosition;
        SetWidthRope();
        _offsetRopeView = new Vector3(0,0, _startRopeViewTransform.position.z);
    }

    private void ReturnStartState()
    {
        _connectCollider.transform.localPosition = _startConnectColliderPosition;
        _distanceJoint.connectedBody = _connectCollider.attachedRigidbody;
    }

    public void Update()
    {
        _lineRenderer.SetPosition(0, _startRopeViewTransform.position);
        _lineRenderer.SetPosition(1, _distanceJoint.connectedBody.gameObject.transform.position + _offsetRopeView);
    }

    private void SetWidthRope()
    {
        _lineRenderer.startWidth = _widthRope;
        _lineRenderer.endWidth = _widthRope;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
