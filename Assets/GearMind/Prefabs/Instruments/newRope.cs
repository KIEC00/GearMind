using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.GearMind.Input;
using Assets.GearMind.Level;
using Assets.GearMind.Objects;
using Assets.Utils.Runtime;
using EditorAttributes;
using NUnit.Framework;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;



public class newRope : MonoBehaviour, IPointerEnterHandler, IGameplayObject
{
    private List<Collider2D> _colliders = new();

    [SerializeField]
    private float _widthRope = 0.2f;

    [SerializeField, Required]
    private LineRenderer _mainLineRenderer;

    [SerializeField, Required]
    private LineRenderer _secondLineRenderer;

    [SerializeField, Required]
    private GameObject _ropePhysics;

    [SerializeField, Required]
    private Collider2D _collider;

    [SerializeField, Required]
    private Collider2D _connector;

    [SerializeField, Required]
    private Renderer _renderer;

    private bool _isCanConnect = true;
    private bool _isCanBreak;
    private int _indexCutCollider = -1;
    private LevelLifetimeScope _levelLifetimeScope;
    private InputCutRope _testService;

    private IDisposable _disposed;


    private void FindColliders()
    {
        for (var i = 0; i < _ropePhysics.transform.childCount; i++)
        {
            var child = _ropePhysics.transform.GetChild(i);
            var ncollider = child.GetComponent<Collider2D>();
            _colliders.Add(ncollider);
        }
    }

    public void FindInputSystem()
    {
        _levelLifetimeScope = FindFirstObjectByType<LevelLifetimeScope>();
        _testService = _levelLifetimeScope.Container.Resolve<InputCutRope>();
    }

    private void DrawLine()
    {
        if (!_mainLineRenderer.enabled)
            return;
        if(_indexCutCollider < 0)
        {
            for (var i = 0; i < _colliders.Count; i++)
            {
                _mainLineRenderer.SetPositions(_colliders.Where(x => x.gameObject.IsActive()).Select(x => x.transform.position).ToArray());
            }
        }
        else
        {
            _mainLineRenderer.positionCount = _indexCutCollider;
            for(var i = 0; i < _indexCutCollider; i++)
            {
                _mainLineRenderer.SetPositions(_colliders.Take(_indexCutCollider).Select(x => x.transform.position).ToArray()); 
            }
            _secondLineRenderer.positionCount = _colliders.Count - _indexCutCollider - 1;
            for(var i = 0; i < _secondLineRenderer.positionCount; i++)
            {
                _secondLineRenderer.SetPositions(_colliders.Skip(_indexCutCollider + 1).Select(x => x.transform.position).ToArray());
            }

        }
        
    }

    public void ConnectInstrument(Collider2D collider)
    {
        if (_isCanConnect && collider.gameObject.TryGetComponent<IGameplayObject>(out _))
        {
            var fixedJoint = _connector.GetComponent<DistanceJoint2D>();
            fixedJoint.enabled = true;
            fixedJoint.connectedBody = collider.attachedRigidbody;
            var distanceJoint = _colliders[0].GetComponent<DistanceJoint2D>();
            distanceJoint.connectedBody = collider.attachedRigidbody;
            distanceJoint.enabled = true;
            
            _isCanBreak = true;
            _isCanConnect = false;
        }


    }

    public void RegisterMethods()
    {
        _disposed = _connector.OnTriggerEnter2DAsObservable().Subscribe(collider => ConnectInstrument(collider)).AddTo(this);
    }



    private void SettingRope()
    {
        for (var i = 0; i < _colliders.Count - 1; i++)
        {
            _colliders[i].GetComponent<FixedJoint2D>().connectedBody = _colliders[i + 1].attachedRigidbody;
        }

        
        
    }
    public void Awake()
    {

        FindColliders();
        _mainLineRenderer.positionCount = _colliders.Count;
        _mainLineRenderer.startWidth = _widthRope;
        _mainLineRenderer.endWidth = _widthRope;
        _secondLineRenderer.startWidth = _widthRope;
        _secondLineRenderer.endWidth = _widthRope;
        SettingRope();
        RegisterMethods();
    }

    public void Method()
    {
        
    }
    public void Start()
    {
        FindInputSystem();
    }
    public void Update()
    {
        DrawLine();
    }

    public void CutRope()
    {

    }

    public void CutIndex(int index)
    {
        _colliders[index].gameObject.SetActive(false);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isCanBreak && _testService.GetIsDraging())
        {
            var enterCollider = eventData.pointerEnter.GetComponent<Collider2D>();
            _secondLineRenderer.enabled = true;
            _indexCutCollider = _colliders.IndexOf(enterCollider);
            _secondLineRenderer.positionCount = _colliders.Count - _indexCutCollider - 1;
            CutIndex(_indexCutCollider);
            var distanceJoint = _colliders[0].GetComponent<DistanceJoint2D>();
            distanceJoint.connectedBody.angularVelocity = 0;
            distanceJoint.connectedBody = null;
            distanceJoint.enabled = false;

            _isCanBreak = false;
            
            StartCoroutine(DissolveRope());
        }
    }

    public void EnterEditMode()
    {
        this.StopAllCoroutines();
        
        SettingRopeEdit();
        _isCanConnect = false;
        _isCanBreak = false;
        _collider.enabled = true;
        _mainLineRenderer.enabled = true;

    }

    public void EnterPlayMode()
    {
        ActiveRope();
        _collider.enabled = false;
        _isCanConnect = true;
    }

    public void SettingRopeEdit()
    {
        var connector = _colliders[_colliders.Count - 1].GetComponent<DistanceJoint2D>();
        connector.connectedBody = null;
        connector.enabled = false;

        var distanceJoint = _colliders[0].GetComponent<DistanceJoint2D>();
        distanceJoint.connectedBody = null;
        distanceJoint.enabled = false;

        for (var i = 0; i < _colliders.Count; i++)
        {
            _colliders[i].attachedRigidbody.bodyType = RigidbodyType2D.Static;
            _colliders[i].transform.localPosition = new Vector3(0, -i * 2, 0);
            _colliders[i].transform.rotation = Quaternion.identity;
            if(i != _colliders.Count - 1)
                _colliders[i].GetComponent<FixedJoint2D>().enabled = false;
            _colliders[i].enabled = false;
            _colliders[i].gameObject.SetActive(true);
        }

        if (_indexCutCollider >= 0)
            _colliders[_indexCutCollider].gameObject.SetActive(true);


        var color = _mainLineRenderer.material.color;
        _mainLineRenderer.material.color = new Color(color.r, color.g, color.b, 1);
        _secondLineRenderer.material.color = new Color(color.r, color.g, color.b, 1);

        _indexCutCollider = -1;

        _secondLineRenderer.enabled = false;
        _mainLineRenderer.positionCount = _colliders.Count;
    }

    public void ActiveRope()
    {
        for (var i = 0; i < _colliders.Count; i++)
        {
            _colliders[i].enabled = true;
            _colliders[i].attachedRigidbody.bodyType = RigidbodyType2D.Dynamic;
            if (i != _colliders.Count - 1)
                _colliders[i].GetComponent<FixedJoint2D>().enabled = true;
        }
    }

    public IEnumerator DissolveRope()
    {
        var alpha = 1f;
        var color = _mainLineRenderer.material.color;
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
        while (alpha > 0)
        {
            color.a = alpha;
            _mainLineRenderer.material.color = new Color(color.r, color.g, color.b, alpha);
            _secondLineRenderer.material.color = new Color(color.r, color.g, color.b, alpha);
            alpha -= 0.1f;
            yield return new WaitForSeconds(0.2f);
        }
        _mainLineRenderer.enabled = false;
        _secondLineRenderer.enabled = false;
        foreach(var collider in _colliders)
        {
            collider.gameObject.SetActive(false);
        }
    }
}

