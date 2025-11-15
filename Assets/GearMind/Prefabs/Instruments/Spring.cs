using System;
using Assets.GearMind.Instruments;
using EditorAttributes;
using R3;
using R3.Triggers;
using UnityEngine;

public class Spring : MonoBehaviour, IGameplayObject, INotConnectedObject
{
    [Header("Force")]
    [SerializeField]
    private float _forseSpring;

    [Header("")]
    [SerializeField, Required]
    private Collider2D _pushCollider;

    [SerializeField, Required]
    private Collider2D _collider;

    private IDisposable _disposable;

    private void RegisterMethods()
    {
        if (_disposable != null)
            _disposable.Dispose();
        _disposable = _pushCollider
            .OnTriggerEnter2DAsObservable()
            .Subscribe(collisionCollider => PushObject(collisionCollider))
            .AddTo(this);
    }

    public void PushObject(Collider2D collider)
    {
        collider.attachedRigidbody.AddForce(
            -collider.attachedRigidbody.linearVelocity * transform.up,
            ForceMode2D.Impulse
        );
        collider.attachedRigidbody.AddForce(transform.up * _forseSpring, ForceMode2D.Impulse);
    }

    public void EnterEditMode()
    {
        _collider.enabled = true;
        _pushCollider.enabled = false;
    }

    public void EnterPlayMode()
    {
        _collider.enabled = false;
        _pushCollider.enabled = true;
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
