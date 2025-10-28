using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Inventory
{
    public class InventoryIdentityComponent : MonoBehaviour
    {
        public IInventoryIdentity InventoryIdentity => _inventoryIdentitySO;

        [SerializeField, Required]
        private InventoryIdentitySO _inventoryIdentitySO;

        void Awake() => enabled = false;
    }
}
