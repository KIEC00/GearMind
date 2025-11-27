using UnityEngine;

namespace Assets.GearMind.Inventory
{
    public interface IInventoryIdentity
    {
        string Name { get; }
        Sprite Icon { get; }
    }
}
