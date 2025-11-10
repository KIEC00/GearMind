using System.Runtime.CompilerServices;
using Assets.GearMind.Test;
using Unity.VisualScripting;
using UnityEngine;

public class DownConnectRigidObject : DefaultRigidObject
{
    [SerializeField]
    private ContactFilter2D _filterConnect;

    private readonly RaycastHit2D[] _hits = new RaycastHit2D[3];

    public override bool ValidatePlacement()
    {
        return base.ValidatePlacement() && CheckConnect();
    }

    private bool CheckConnect()
    {
        var cust = _collider.Cast(Vector2.down, _filterConnect, _hits, 0.3f);
        if ( cust > 0 )
        {
            for(var i = 0; i < cust; i++)
            {
                if (!_hits[i].collider.gameObject.TryGetComponent<INotConnectedObject>(out _))
                    return true;
            }
            
        }
        return false;
    } 

}
