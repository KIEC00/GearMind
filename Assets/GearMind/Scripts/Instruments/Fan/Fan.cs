using Assets.GearMind.Instruments;
using EditorAttributes;
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
    private AreaEffector2D _effector2D;

    [SerializeField, Required]
    private Renderer _renderer;

    private Color _initialColor;

    public bool IsActive { get; private set; } = false;

    private void Awake()
    {
        _initialColor = _renderer.material.color;
        _effector2D.forceMagnitude = _forceFan;
        if (_isNeedTurnOn)
            SetActive(true);
    }

    public void EnterEditMode()
    {
        SetActive(_isNeedTurnOn);
    }

    public void SetActive(bool isTurnOn)
    {
        _fanEffectCollider.enabled = isTurnOn;
        IsActive = isTurnOn;
        if (isTurnOn)
            _renderer.material.color = Color.green;
        else
            _renderer.material.color = _initialColor;
    }

    public void EnterPlayMode() { }

    public void Rotate() => transform.Rotate(new Vector3(0f, 0f, -90f));
}
