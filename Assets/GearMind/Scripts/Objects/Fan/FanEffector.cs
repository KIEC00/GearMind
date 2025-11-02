using EditorAttributes;
using UnityEngine;

public class FanEffector : MonoBehaviour
{
    [SerializeField, Required]
    private BoxCollider2D _collider2D;

    [SerializeField, Required]
    private AreaEffector2D _effector2D;

    private void Awake() => enabled = false;

    public void SetLength(float length)
    {
        length = Mathf.Max(1f, length);
        var scale = transform.localScale;
        var position = transform.localPosition;
        transform.localScale = new Vector3(length, scale.y, scale.z);
        transform.localPosition = new Vector3(length / 2f + 0.5f, position.y, position.z);
    }

    public void SetForce(float force) => _effector2D.forceMagnitude = force;

    public void Disable() => gameObject.SetActive(false);

    public void Enable() => gameObject.SetActive(true);
}
