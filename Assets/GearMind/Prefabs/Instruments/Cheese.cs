using Assets.GearMind.Instruments;
using EditorAttributes;
using UnityEngine;

[SelectionBase]
public class Cheese : MonoBehaviour, IGameplayObject, IDamageable
{
    [SerializeField, Required]
    private Collider2D _collider;

    [SerializeField, Required]
    private Rigidbody2D _rigidbody;

    [SerializeField, Required]
    private GameObject _cheeseView;

    public void EnterEditMode()
    {
        _cheeseView.SetActive(true);
        _collider.enabled = true;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    public void EnterPlayMode()
    {
        _collider.attachedRigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    public void ApplyDamage()
    {
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _collider.enabled = false;
        _cheeseView.SetActive(false);
    }
}
