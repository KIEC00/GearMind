using System.Collections;
using Assets.GearMind.Instruments;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;

[SelectionBase]
public class Laser : MonoBehaviour, IGameplayObject, ISwitchable, INotConnectedObject
{
    public bool IsActive { get; private set; }

    [Header("Params")]
    [SerializeField, Clamp(0f, 10_000f)]
    private float _maxDistance = float.MaxValue;

    [SerializeField, OnValueChanged(nameof(DebugToggle))]
    private bool _isNeedTurnOn = false;

    [SerializeField, Required]
    private LaserBeam _laserBeam;

    [Header("Hits detections")]
    [SerializeField]
    private ContactFilter2D _filterLaserHit;

    [SerializeField]
    private Vector3 _castOffset;

    private readonly RaycastHit2D[] _laserHits = new RaycastHit2D[1];
    private Coroutine _updateLaserCoroutine;
    private readonly YieldInstruction _waitFixedFrameInstruction = new WaitForFixedUpdate();

    private Vector2 CastStart => transform.position + transform.rotation * _castOffset;

    private void Awake() => SetActive(_isNeedTurnOn);

    public IEnumerator UpdateLaserRoutine()
    {
        while (true)
        {
            PerformCast();
            yield return _waitFixedFrameInstruction;
        }
    }

    public void PerformCast()
    {
        int hitCount = RayCast();

        if (hitCount > 0)
        {
            var hitPosition = _laserHits[0].point;
            UpdateBeam(hitPosition);

            if (_laserHits[0].collider.TryGetComponent<IDamageable>(out var damageable))
                damageable.ApplyDamage();

            return;
        }

        UpdateBeam(transform.right * _maxDistance);
    }

    private int RayCast() =>
        Physics2D.Raycast(CastStart, transform.right, _filterLaserHit, _laserHits, _maxDistance);

    private void UpdateBeam(Vector2 hitPosition) =>
        _laserBeam.UpdateBeam(CastStart, hitPosition.WithZ(_castOffset.z));

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        _laserBeam.enabled = isActive;

        if (IsActive)
            _updateLaserCoroutine = StartCoroutine(UpdateLaserRoutine());
        else if (_updateLaserCoroutine != null)
            StopCoroutine(_updateLaserCoroutine);
    }

    public void EnterEditMode()
    {
        SetActive(_isNeedTurnOn);
    }

    public void EnterPlayMode() { }

    private void DebugToggle()
    {
        if (Application.isPlaying)
            SetActive(_isNeedTurnOn);
    }
}
