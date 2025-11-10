using System.Collections;
using System.Collections.Generic;
using Assets.GearMind.Objects;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;

public class Laser : MonoBehaviour, IGameplayObject, IIncludedObject, INotConnectedObject
{
    [Header("LaserParameters")]
    [SerializeField]
    private int _maxDistance = 3;

    [SerializeField]
    private float _laserWidth = 0.1f;

    [SerializeField]
    private bool _isNeedTurnOn = false;

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


    private List<RaycastHit2D> _laserHits = new ();
    private Vector3 _offsetDrawLaserVector;

    private Coroutine _updateLaserCoroutine;

    public bool IsTurnOn { get; private set; }



    //добавить обработку столкновения с сыром(когда будет сыр)
    public IEnumerator UpdateLaser()
    {
        ICheese cheese;
        while (true)
        {
            var hit = Physics2D.Raycast(_startLaserPoint.position, transform.right, _filterLaserHit, _laserHits, _maxDistance);
            if (hit > 0 && _laserHits[0].collider != null)
            {
                _lineRenderer.SetPosition(0, _startLaserPoint.position + _offsetDrawLaserVector);
                _lineRenderer.SetPosition(1, new Vector3(_laserHits[0].point.x, _laserHits[0].point.y, _offsetDrawLaserVector.z));
                
                if (_laserHits[0].collider.TryGetComponent<ICheese>(out cheese))
                {
                    cheese.DestroyCheese();
                    _laserHits.Clear();
                }
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
        gameObject.SetActive(false);
    }


    public void Awake()
    {
        _offsetDrawLaserVector = new Vector3(0,0,_offsetDrawLaser.position.z);
        _lineRenderer.startWidth = _laserWidth;
        _lineRenderer.endWidth = _laserWidth;
        if (_isNeedTurnOn)
            TurnOnOff(true);

    }


    public void TurnOnOff(bool isActive)
    {
        IsTurnOn = isActive;
        if (IsTurnOn)
        {
            _lineRenderer.enabled = true;
            
            _updateLaserCoroutine = StartCoroutine(UpdateLaser());
            
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

    public void EnterEditMode()
    {

        TurnOnOff(_isNeedTurnOn);
    }

    public void EnterPlayMode()
    {
    }
}
