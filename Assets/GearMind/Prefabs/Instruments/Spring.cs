using EditorAttributes;
using UnityEngine;

public class Spring : DownConnectRigidObject
{
    [SerializeField, Required]
    private Collider2D _pushCollider;


    public override void EnterEditMode()
    {
        base.EnterEditMode();
        _collider.enabled = true;
        _pushCollider.enabled = false;
    }

    public override void EnterPlayMode()
    {
        _collider.enabled = false;
        _pushCollider.enabled = true;   
    }
}
