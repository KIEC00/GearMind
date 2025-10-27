using System;
using System.Collections.Generic;

namespace Assets.GearMind.Inventory
{
    public interface IInventory : IEnumerable<KeyValuePair<IInventoryIdentity, int>>
    {
        int this[IInventoryIdentity data] { get; set; }
        event Action<InventoryChangeEventData> OnChange;
    }

    public readonly struct InventoryChangeEventData
    {
        public IInventoryIdentity Data { get; }
        public int PreviousCount { get; }
        public int CurrentCount { get; }
        public int Delta => CurrentCount - PreviousCount;

        public InventoryChangeEventData(
            IInventoryIdentity data,
            int previousValue,
            int currentValue
        )
        {
            Data = data;
            PreviousCount = previousValue;
            CurrentCount = currentValue;
        }
    }
}
