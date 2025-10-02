using System.Collections.Generic;
using System.Linq;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Grid.Components
{
    public partial class GridComponent : MonoBehaviour, ISerializationCallbackReceiver
    {
        [field: SerializeField, Clamp(1, 1000, 1, 1000)]
        public Vector2Int Size { get; private set; } = Vector2Int.one;

        [field: SerializeField, Clamp(0.01f, 100f, 0.01f, 100f)]
        public float CellWorldSize { get; private set; } = 1f;

        public Vector3 WorldCenter => transform.position;
        public Vector2 WorldSize => (Vector2)Size * CellWorldSize;
        public Vector2 WorldExtends => WorldSize / 2f;

        public Rect WordRect => new((Vector2)WorldCenter - WorldExtends, WorldSize);
        public Bounds WorldBounds => new(WorldCenter, WorldSize);

        public Grid Cells { get; private set; }

        private Dictionary<AbstractGridItemComponent, GridItem> _itemComponents = new();

        public bool AddItem(AbstractGridItemComponent itemComponent, Vector2Int position)
        {
            if (_itemComponents.ContainsKey(itemComponent))
                return false;
            var gridItem = new GridItem(itemComponent.Cells, itemComponent);
            var added = Cells.AddItem(gridItem, position);
            if (added)
                _itemComponents.Add(itemComponent, gridItem);
            return added;
        }

        public bool CanAddItem(AbstractGridItemComponent itemComponent, Vector2Int position)
        {
            if (_itemComponents.ContainsKey(itemComponent))
                return false;
            var gridItem = new GridItem(itemComponent.Cells, itemComponent);
            return Cells.CanAddItem(gridItem, position);
        }

        public (int count, IEnumerable<AbstractGridItemComponent> targets) RemoveItemsRecursive(
            AbstractGridItemComponent itemComponent
        )
        {
            if (!_itemComponents.TryGetValue(itemComponent, out var gridItem))
                return (0, null);
            var gridItems = Cells.RemoveItemsRecursive(gridItem);
            return (
                gridItems.Count,
                gridItems.Select(item => (AbstractGridItemComponent)item.Payload)
            );
        }

        public void OnAfterDeserialize() => Cells = new Grid(Size.x, Size.y);

        public void OnBeforeSerialize() { }
    }
}
