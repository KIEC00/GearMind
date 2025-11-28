using System;
using Assets.GearMind.Instruments;
using EditorAttributes;
using R3;
using R3.Triggers;
using UnityEngine;

[SelectionBase]
public class Fan : MonoBehaviour, IGameplayObject, ISwitchable, INotConnectedObject, IRotatable
{
    [SerializeField]
    private float _forceFan = 20;

    [SerializeField]
    private bool _isNeedTurnOn = false;

    [SerializeField, Required]
    private Collider2D _fanEffectCollider;


    [SerializeField, Required]
    private Renderer _renderer;

    [SerializeField, Required]
    private GameObject _fanEffectView;

    private Color _initialColor;
    private IDisposable _disposable;

    public bool IsActive { get; private set; } = false;

    private void Awake()
    {
        _initialColor = _renderer.material.color;
        
        if (_isNeedTurnOn)
            SetActive(true);
    }

    public void RegisterMethods()
    {
        _disposable = _fanEffectCollider.OnTriggerStay2DAsObservable().Subscribe(colliderPush => PushInstrument(colliderPush)).AddTo(this);
    }

    public void PushInstrument(Collider2D collider)
    {
        if (!collider.isTrigger)
            collider.attachedRigidbody.AddForce(transform.right * _forceFan, ForceMode2D.Force);
    }

    public void EnterEditMode()
    {
        SetActive(_isNeedTurnOn);
    }

    public void SetActive(bool isTurnOn)
    {
        _fanEffectCollider.enabled = isTurnOn;
        IsActive = isTurnOn;
        _fanEffectView.SetActive(isTurnOn);
        if (isTurnOn)
            _renderer.material.color = Color.green;
        else
            _renderer.material.color = _initialColor;
    }

    

    public void EnterPlayMode() { }

    public void Rotate() => transform.Rotate(new Vector3(0f, 0f, -90f));

    public void OnEnable()
    {
        RegisterMethods();
    }

    public void OnDisable()
    {
        _disposable.Dispose();
    }
}
