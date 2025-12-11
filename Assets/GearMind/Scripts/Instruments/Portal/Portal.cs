using System.Collections;
using Assets.GearMind.Instruments;
using EditorAttributes;
using UnityEngine;

[SelectionBase]
public class Portal : MonoBehaviour, IGameplayObject
{
    private static readonly WaitForSeconds _waitForSeconds0_1 = new(0.1f);

    [SerializeField, Required]
    private Portal _toPortal;

    [Space]
    [SerializeField]
    private Collider2D _collider;

    private bool _isTeleporting;

    public void EnterEditMode() { }

    public void EnterPlayMode() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTeleporting || _toPortal == null)
            return;

        if (!other.TryGetComponent<Rigidbody2D>(out var rb))
            return;

        _isTeleporting = true;
        _toPortal._isTeleporting = true;

        var originalVelocity = rb.linearVelocity;
        var localDirection = transform.InverseTransformDirection(originalVelocity);
        var newDirection = _toPortal.transform.TransformDirection(localDirection);

        other.transform.position = _toPortal.transform.position;
        rb.linearVelocity = newDirection;

        StartCoroutine(ResetTeleportFlagCoroutine());
    }

    private IEnumerator ResetTeleportFlagCoroutine()
    {
        yield return _waitForSeconds0_1;

        _isTeleporting = false;
        if (_toPortal != null)
            _toPortal._isTeleporting = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (!_toPortal)
            return;
        Gizmos.color = Color.aliceBlue;
        Gizmos.DrawLine(transform.position, _toPortal.transform.position);
    }
}
