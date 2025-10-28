using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GearMind.Inventory
{
    public class IdentityPrefabMap : IIdentityPrefabMap
    {
        public GameObject this[IInventoryIdentity identity] => _prefabs.GetValueOrDefault(identity);
        public IEnumerable<IInventoryIdentity> Keys => _prefabs.Keys;
        public IEnumerable<GameObject> Values => _prefabs.Values;
        public int Count => _prefabs.Count;

        private readonly Dictionary<IInventoryIdentity, GameObject> _prefabs;

        public IdentityPrefabMap(
            IEnumerable<KeyValuePair<IInventoryIdentity, GameObject>> prefabs
        ) => _prefabs = new(prefabs);

        public bool ContainsKey(IInventoryIdentity key) => _prefabs.ContainsKey(key);

        public bool TryGetValue(IInventoryIdentity key, out GameObject value) =>
            _prefabs.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<IInventoryIdentity, GameObject>> GetEnumerator() =>
            _prefabs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
