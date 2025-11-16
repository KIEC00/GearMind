using System;
using Assets.GearMind.Objects;
using EditorAttributes;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fan : MonoBehaviour,IGameplayObject, IIncludedObject, INotConnectedObject
{

    [SerializeField]
    private float _forceFan = 20;

    [SerializeField]
    private bool _isNeedTurnOn = false;

    [Header("")]
    [SerializeField, Required]
    private Collider2D _effectCollider;

    [SerializeField, Required]
    private Renderer _renderer;

    [SerializeField, Required]
    private GameObject _viewEffect;

    private Color _initialColor;

    private IDisposable _disposable;


    public bool IsTurnOn { get; private set; } = false;

    private void Awake()
    {
        _initialColor = _renderer.material.color;
        if(_isNeedTurnOn)
            TurnOnOff(true);
    }

    public  void EnterEditMode()
    {
        TurnOnOff(_isNeedTurnOn);
    }

    public void PushObject(Collider2D collider)
    {
        if(!collider.isTrigger)
            collider.attachedRigidbody.AddForce(transform.right * _forceFan, ForceMode2D.Force);
    }

    public void RegisterMethods()
    {
        _disposable = _effectCollider.OnTriggerStay2DAsObservable().Subscribe(collider => PushObject(collider)).AddTo(gameObject);
    }


    public void TurnOnOff(bool isTurnOn)
    {
        _effectCollider.enabled = isTurnOn;
        _viewEffect.SetActive(isTurnOn);
        IsTurnOn = isTurnOn;
        if (isTurnOn)
            _renderer.material.color = Color.green;
        else
            _renderer.material.color = _initialColor;
    }

    public void EnterPlayMode()
    {

    }

    public void OnEnable()
    {
        RegisterMethods();
    }

    public void OnDisable()
    {
        _disposable.Dispose();
    }
}
