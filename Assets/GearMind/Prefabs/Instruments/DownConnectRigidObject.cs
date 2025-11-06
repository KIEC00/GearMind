using System.Runtime.CompilerServices;
using Assets.GearMind.Test;
using Unity.VisualScripting;
using UnityEngine;

public class DownConnectRigidObject : DefaultRigidObject
{
    private readonly RaycastHit2D[] _hits = new RaycastHit2D[3];

    public override bool ValidatePlacement()
    {
        return base.ValidatePlacement() && CheckConnect();
    }
    public override void EnterPlayMode()
    {
       
    }

    private bool CheckConnect()
    {
        var cust = _collider.Cast(Vector2.down, _contactFilter, _hits, 0.3f);
        if ( cust > 0 )
        {
            for(var i = 0; i < cust; i++)
            {
                if (_hits[i].collider.gameObject.TryGetComponent<ConnectObject>(out _))
                    return true;
            }
            
        }
        return false;
    } 

}
