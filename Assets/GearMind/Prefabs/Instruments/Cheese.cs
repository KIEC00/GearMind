using Assets.GearMind.Objects;
using EditorAttributes;
using UnityEngine;

public class Cheese : MonoBehaviour, IGameplayObject, ICheese
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

    public void DestroyCheese()
    {
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _collider.enabled = false;
        _cheeseView.SetActive(false);
    }
    
}
