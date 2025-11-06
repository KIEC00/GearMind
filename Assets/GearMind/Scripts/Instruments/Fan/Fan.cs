using EditorAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fan : VertiHorizConnectRigidObject, IIncludedObject
{

    [SerializeField]
    private float _forceFan = 20;

    [Header("")]
    [SerializeField, Required]
    private Collider2D _fanEffectCollider;

    [SerializeField, Required]
    private AreaEffector2D _effector2D;

    [SerializeField, Required]
    private Renderer _renderer;

    private Color _initialColor;

    public bool IsTurnOn { get; private set; } = false;

    private void Awake()
    {
        _initialColor = _renderer.material.color;
        _effector2D.forceMagnitude = _forceFan;
    }

    public override void EnterEditMode()
    {
        base.EnterEditMode();
        TurnOnOff(false);
    }


    public void TurnOnOff(bool isTurnOn)
    {
        _fanEffectCollider.enabled = isTurnOn;
        IsTurnOn = isTurnOn;
        if (isTurnOn)
            _renderer.material.color = Color.green;
        else
            _renderer.material.color = _initialColor;
    }


    private void OnEnable()
    {
        TurnOnOff(false);
    }

    private void OnDisable()
    {
        TurnOnOff(false);
    }
}
