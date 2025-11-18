using System.Collections;
using UnityEngine;

[SelectionBase]
public class Portal : MonoBehaviour
{
    [SerializeField] 
    private Portal _toPortal;

    private bool _isTeleporting;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Cheese") || _isTeleporting || _toPortal == null)
            return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null) return;

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
        yield return new WaitForSeconds(0.1f);
        
        _isTeleporting = false;
        if (_toPortal != null)
            _toPortal._isTeleporting = false;
    }
}