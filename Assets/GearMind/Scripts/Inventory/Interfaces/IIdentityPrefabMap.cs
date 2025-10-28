using System.Collections.Generic;
using UnityEngine;

namespace Assets.GearMind.Inventory
{
    public interface IIdentityPrefabMap : IReadOnlyDictionary<IInventoryIdentity, GameObject> { }
}
