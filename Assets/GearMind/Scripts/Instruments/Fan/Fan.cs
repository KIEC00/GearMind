using Assets.GearMind.Instruments;
using EditorAttributes;
using UnityEngine;

[SelectionBase]
public class Fan : MonoBehaviour, IGameplayObject, ISwitchable, INotConnectedObject, IRotatable
{
    [SerializeField]
    private bool _activateOnStart = false;

#if UNITY_EDITOR
    [SerializeField, Clamp(0f, 1000f), OnValueChanged(nameof(UpdateForce))]
    private float _effectorForce = 20;

    [SerializeField, Clamp(0f, 100f), OnValueChanged(nameof(UpdateSize))]
    private float _effectorSize = 4;
#endif

    [SerializeField, Required]
    private Collider2D _fanEffectCollider;

    [SerializeField, Required]
    private AreaEffector2D _effector2D;

    [SerializeField, Required]
    private Renderer _renderer;

    [SerializeField, Required]
    private FanParticleControls _particles;

    private Color _initialColor;

    public bool IsActive { get; private set; } = false;

    private void Awake()
    {
        _initialColor = _renderer.material.color;
        _particles.StopEmmiting();
        if (_activateOnStart)
            SetActive(true);
    }

    public void PushInstrument(Collider2D collider)
    {
        if (!collider.isTrigger)
            collider.attachedRigidbody.AddForce(
                transform.right * _effectorForce,
                ForceMode2D.Force
            );
    }

    public void EnterEditMode()
    {
        SetActive(_activateOnStart);
    }

    public void SetActive(bool isTurnOn)
    {
        _fanEffectCollider.enabled = isTurnOn;
        IsActive = isTurnOn;
        if (isTurnOn)
        {
            _renderer.material.color = Color.green;
            _particles.StartEmmiting();
        }
        else
        {
            _renderer.material.color = _initialColor;
            _particles.StopEmmiting();
        }
    }

    public void EnterPlayMode() { }

    public void Rotate() => transform.Rotate(new Vector3(0f, 0f, -90f));

    private void UpdateForce()
    {
        _effector2D.forceMagnitude = _effectorForce;
    }

    private void UpdateSize()
    {
        var effectorScale = _effector2D.transform.localScale;
        var effectorPosition = _effector2D.transform.localPosition;

        effectorScale.x = _effectorSize;
        effectorPosition.x = _effectorSize / 2f + .5f;

        _effector2D.transform.localScale = effectorScale;
        _effector2D.transform.localPosition = effectorPosition;

        _particles.SetSize(_effectorSize);
    }
}
