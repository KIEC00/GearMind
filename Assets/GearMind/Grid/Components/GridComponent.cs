using System.Collections.Generic;
using System.Linq;
using EditorAttributes;
using Mono.Cecil.Cil;
using UnityEngine;

namespace Assets.GearMind.Grid.Components
{
    public partial class GridComponent : MonoBehaviour, ISerializationCallbackReceiver
    {
        [field: SerializeField, Clamp(1, 1000, 1, 1000)]
        public Vector2Int Size { get; private set; } = Vector2Int.one;

        [field: SerializeField, Clamp(0.01f, 100f, 0.01f, 100f)]
        public float CellScale { get; private set; } = 1f;

        public Vector3 WorldCenter => transform.position;
        public Vector2 WorldSize => (Vector2)Size * CellScale;
        public Vector2 WorldExtends => WorldSize / 2f;

        public Rect WordRect => new((Vector2)WorldCenter - WorldExtends, WorldSize);
        public Bounds WorldBounds => new(WorldCenter, WorldSize);

        public Grid Cells { get; private set; }

        private readonly Dictionary<IGridItemComponent, GridItem> _itemComponents = new();

        public bool AddItem(IGridItemComponent itemComponent, Vector2Int position)
        {
            if (_itemComponents.ContainsKey(itemComponent))
                return false;
            var gridItem = new GridItem(itemComponent.Cells, position, itemComponent);
            var added = Cells.AddItem(gridItem, position);
            if (added)
                _itemComponents.Add(itemComponent, gridItem);
            return added;
        }

        public bool AddItem(
            IGridItemComponent itemComponent,
            Vector2Int position,
            out IEnumerable<GridItem> attachedTo
        )
        {
            attachedTo = null;
            if (_itemComponents.ContainsKey(itemComponent))
                return false;
            var gridItem = new GridItem(itemComponent.Cells, position, itemComponent);
            var added = Cells.AddItem(gridItem, position, out attachedTo);
            if (added)
                _itemComponents.Add(itemComponent, gridItem);
            return added;
        }

        public bool CanAddItem(IGridItemComponent itemComponent, Vector2Int position)
        {
            if (_itemComponents.ContainsKey(itemComponent))
                return false;
            var gridItem = new GridItem(itemComponent.Cells, position, itemComponent);
            return Cells.CanAddItem(gridItem, position);
        }

        public IReadOnlyCollection<GridItem> RemoveItemsRecursive(IGridItemComponent itemComponent)
        {
            if (!_itemComponents.TryGetValue(itemComponent, out var gridItem))
                return null;
            return Cells.RemoveItemsRecursive(gridItem);
        }

        public IReadOnlyCollection<GridItem> RemoveItemsRecursive(GridItem item) =>
            Cells.RemoveItemsRecursive(item);

        public GridItem GetSolidItemAt(Vector2Int position) =>
            Cells[position.x, position.y].GetSolidRecord()?.Item;

        public IEnumerable<GridItem> GetItemsAt(Vector2Int position) =>
            Cells[position.x, position.y].Select(r => r.Item);

        public void OnAfterDeserialize() => Cells = new Grid(Size.x, Size.y);

        public void OnBeforeSerialize() { }
    }
}
