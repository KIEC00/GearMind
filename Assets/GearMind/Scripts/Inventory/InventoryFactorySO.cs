using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Assets.GearMind.Inventory
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "GearMind/Inventory", order = -1)]
    public class InventoryFactorySO : ScriptableObject, IInventoryFactory
    {
        [SerializeField, SerializedDictionary("Prefab", "Count")]
        private SerializedDictionary<InventoryIdentityComponent, int> _inventory;

        public IInventory CreateInventory() =>
            new Inventory(
                _inventory.Select(kvp => new KeyValuePair<IInventoryIdentity, int>(
                    kvp.Key.InventoryIdentity,
                    kvp.Value
                ))
            );
    }
}
