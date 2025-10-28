using UnityEngine;

namespace Assets.GearMind.Inventory
{
    [CreateAssetMenu(
        fileName = "InventoryIdentity",
        menuName = "GearMind/Inventory Identity",
        order = -1
    )]
    public class InventoryIdentitySO : ScriptableObject, IInventoryIdentity
    {
        [field: SerializeField]
        public string Name { get; private set; }
    }
}
