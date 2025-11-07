using System.Collections;
using EditorAttributes;
using UnityEngine;

public class Laser : VertiHorizConnectRigidObject, IIncludedObject
{
    [Header("LaserParameters")]
    [SerializeField]
    private int _maxDistance = 3;

    [SerializeField]
    private float _laserWidth = 0.1f;

    [Header("FilterLaserHit")]
    [SerializeField]
    private ContactFilter2D _filterLaserHit;

    [Header("")]
    [SerializeField, Required]
    private Transform _startLaserPoint;

    [SerializeField, Required]
    private Transform _offsetDrawLaser;
    
    [SerializeField, Required]
    private LineRenderer _lineRenderer;


    private RaycastHit2D[] _laserHits = new RaycastHit2D[1];
    private Vector3 _offsetDrawLaserVector;

    private Coroutine _updateLaserCoroutine;

    public bool IsTurnOn { get; private set; }



    //добавить обработку столкновения с сыром(когда будет сыр)
    public IEnumerator UpdateLaser()
    {

        while (true)
        {
            var hit = Physics2D.Raycast(_startLaserPoint.position, transform.right, _filterLaserHit, _laserHits, _maxDistance);
            var hitCollider = _laserHits[0].collider;
            if (hitCollider != null)
            {
                if (hitCollider.CompareTag("Ball"))
                {
                    DestroyCheese(_laserHits[0].collider.gameObject);
                }
                _lineRenderer.SetPosition(0, _startLaserPoint.position + _offsetDrawLaserVector);
                _lineRenderer.SetPosition(1, new Vector3(_laserHits[0].point.x, _laserHits[0].point.y, _offsetDrawLaserVector.z));
            }
            else
            {
                _lineRenderer.SetPosition(1, _startLaserPoint.position + transform.right * _maxDistance + _offsetDrawLaserVector);
                _lineRenderer.SetPosition(0, _startLaserPoint.position + _offsetDrawLaserVector);

            }
            yield return null;
        }
        
    }

    public void DestroyCheese(GameObject gameObject)
    {
        Destroy(gameObject);
    }


    public void Awake()
    {
        _offsetDrawLaserVector = new Vector3(0,0,_offsetDrawLaser.position.z);
        _lineRenderer.startWidth = _laserWidth;
        _lineRenderer.endWidth = _laserWidth;
    }


    public void TurnOnOff(bool isActive)
    {
        IsTurnOn = isActive;
        if (IsTurnOn)
        {
            _lineRenderer.enabled = true;
            if(_updateLaserCoroutine != null)
            {
                _updateLaserCoroutine = StartCoroutine(UpdateLaser());
            }
        }
        else
        {
            if(_updateLaserCoroutine!= null)
            {
                StopCoroutine(_updateLaserCoroutine);
            }  
            _lineRenderer.enabled = false;
        }
        
    }

    public override void EnterEditMode()
    {
        TurnOnOff(false);
    }
}
