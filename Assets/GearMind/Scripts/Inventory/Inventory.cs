using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.GearMind.Inventory
{
    public class Inventory : IInventory
    {
        public int this[IInventoryIdentity data]
        {
            get => _items.GetValueOrDefault(data, 0);
            set => HandleChange(data, value);
        }
        public event Action<InventoryChangeEventData> OnChange;

        private readonly Dictionary<IInventoryIdentity, int> _items;

        public Inventory(IEnumerable<KeyValuePair<IInventoryIdentity, int>> items) =>
            _items = new(items);

        private void HandleChange(IInventoryIdentity data, int value)
        {
            var previousCount = _items.GetValueOrDefault(data, 0);
            _items[data] = value;
            OnChange?.Invoke(new InventoryChangeEventData(data, previousCount, value));
        }

        public IEnumerator<KeyValuePair<IInventoryIdentity, int>> GetEnumerator() =>
            _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
