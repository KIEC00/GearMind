using System;
using System.Collections.Generic;
using Assets.GearMind.Instruments;
using EditorAttributes;
using R3;
using R3.Triggers;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonInstrument : MonoBehaviour, IGameplayObject
{

    [Header("FilterIncludeCollider")]
    [SerializeField]
    private ContactFilter2D _includeColliderFilter;

    [Header("")]
    [SerializeField, Required]
    private Collider2D _pushCollider;

    [SerializeField, Required]
    private Collider2D _includeCollider;

    [SerializeField, Required]
    private Renderer _rendererRedButton;

    [SerializeField, Required]
    private Collider2D _collider;

    [SerializeField, Required]
    private GameObject _viewRadius;

    

    
    private int CountPushObjects;
    private Collider2D[] _collisionColiders = new Collider2D[30];

    private Color _initialiseColor;

    public  void EnterEditMode()
    {
        _collider.enabled = true;
        _pushCollider.enabled = false;
        _includeCollider.enabled = false;
        _viewRadius.SetActive(true);
    }

    public  void EnterPlayMode()
    {
        _collider.enabled = false;
        _pushCollider.enabled = true;
        _includeCollider.enabled = true;
        _viewRadius.SetActive(false);
    }

    public void Awake()
    {
        RegisterObservable();
        _initialiseColor = _rendererRedButton.material.color;
    }
    public void OnEnable()
    {
        _includeCollider.enabled = false;
    }

    public void OnDisable()
    {
        _includeCollider.enabled = false;
    }


    private void RegisterObservable()
    {
        _pushCollider.OnTriggerEnter2DAsObservable().Subscribe(triger => PressButton(triger)).AddTo(gameObject);
        _pushCollider.OnTriggerExit2DAsObservable().Subscribe(triger => PopButton(triger)).AddTo(gameObject);
    }


    private void PressButton(Collider2D collider)
    {
        if(collider.isTrigger == false)
        {
            CountPushObjects += 1;
            if (CountPushObjects == 1)
            {
                CheckIncludeObjectkCollision();
                _rendererRedButton.material.color = Color.green;
            }
        } 
    }

    private void PopButton(Collider2D collider)
    {
        if (collider.isTrigger == false)
        {
            CountPushObjects -= 1;
            if(CountPushObjects == 0)
            {
                _rendererRedButton.material.color = _initialiseColor;
            }
        }
    }

    

    private void CheckIncludeObjectkCollision()
    {
        var countCollision = _includeCollider.Overlap(_includeColliderFilter, _collisionColiders);
        for(var i = 0; i < countCollision; i++)
        {
            IncludeInstruments(_collisionColiders[i]);
        }
    }

    private void IncludeInstruments(Collider2D collider)
    {
        ISwitchable includedObject;
        if (collider.gameObject.TryGetComponent<ISwitchable>(out includedObject))
        {
            if(!includedObject.IsActive)
                includedObject.Activate();
            else
                includedObject.Deactivate();
        }
    }

   



}
