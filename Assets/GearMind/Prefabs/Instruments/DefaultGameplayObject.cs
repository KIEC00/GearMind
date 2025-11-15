using Assets.GearMind.Instruments;
using EditorAttributes;
using UnityEngine;

public class DefaultGameplayObject : MonoBehaviour, IGameplayObject
{
    [SerializeField]
    private bool _isNeedKinematic;

    [SerializeField, Required]
    private Rigidbody2D _rigidbody;

    public void EnterEditMode()
    {
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    public void EnterPlayMode()
    {
        if (!_isNeedKinematic)
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
