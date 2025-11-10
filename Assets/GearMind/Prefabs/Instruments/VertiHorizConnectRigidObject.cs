using Assets.GearMind.Test;
using UnityEngine;

public class VertiHorizConnectRigidObject : DefaultRigidObject
{
    [SerializeField]
    private ContactFilter2D _filterConnect;

    private readonly RaycastHit2D[] _hitsLeft = new RaycastHit2D[3];
    private readonly RaycastHit2D[] _hitsRight = new RaycastHit2D[3];
    private readonly RaycastHit2D[] _hitsUp = new RaycastHit2D[3];
    private readonly RaycastHit2D[] _hitsDown = new RaycastHit2D[3];
    public override bool ValidatePlacement()
    {
        return base.ValidatePlacement() && CheckConnect();
    }

    private bool CheckConnect()
    {
        return CheckConnectSide(Vector2.left, _hitsLeft)
            || CheckConnectSide(Vector2.right, _hitsRight)
            || CheckConnectSide(Vector2.up, _hitsUp)
            || CheckConnectSide(Vector2.down, _hitsDown);
    }

    private bool CheckConnectSide(Vector2 direction,  RaycastHit2D[] hits)
    {
        var countHits = _collider.Cast(direction, _filterConnect, hits, 0.3f);
        if (countHits > 0)
        {
            for (var i = 0; i < countHits; i++)
            {
                if (!hits[i].collider.gameObject.TryGetComponent<INotConnectedObject>(out _))
                    return true;
            }
        }
        return false;
    }
}
