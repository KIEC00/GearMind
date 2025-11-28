using Assets.GearMind.Test;
using UnityEngine;

public class UpConnectRigidObject : DefaultRigidObject
{

    [SerializeField]
    private ContactFilter2D _filterConnect;

    [SerializeField]
    private ContactFilter2D _secondContactFilter;

    private readonly RaycastHit2D[] _hits = new RaycastHit2D[3];

    public override bool ValidatePlacement()
    {
        return base.ValidatePlacement() && SecondValidate() && CheckConnect();
    }

    private bool SecondValidate()
    {
        return _collider.Cast(Vector2.zero, _secondContactFilter, _hits, 0f) == 0;
    }

    private bool CheckConnect()
    {
        var cust = _collider.Cast(Vector2.up, _filterConnect, _hits, 0.3f);
        if (cust > 0)
        {
            for (var i = 0; i < cust; i++)
            {
                if (!_hits[i].collider.gameObject.TryGetComponent<INotConnectedObject>(out _))
                    return true;
            }

        }
        return false;
    }
}
