using System;
using System.Collections.Generic;
using Assets.GearMind.Grid.Components;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Grid.Tests
{
    public class TestGridItem : AbstractGridObject
    {
        public static int Index = 0;

        [SerializeField]
        private bool _sphereRequiredAttach = false;

        [SerializeField, Required]
        private GameObject _sphere;

        private Transform _prevParent;

        public override IEnumerable<Cell> Cells => GetCells();

        public void Start() => _sphere.SetActive(_sphereRequiredAttach);

        public override bool ValidateAt(Vector2Int position, Grid grid)
        {
            var firstOverlap = grid[position].FirstOrNull(OverlapPredicate);
            var overlap = firstOverlap != null;
            if (!_sphereRequiredAttach || overlap)
                return !overlap;
            var dependency = GetDependsOnItem(position, grid);
            return dependency != null;
        }

        public override void AfterPlace(Vector2Int position, Grid grid)
        {
            if (!_sphereRequiredAttach)
                return;
            var component = GetDependsOnItem(position, grid).Component;
            if (component is not MonoBehaviour monoComponent)
                throw new ArgumentException("Component is not a MonoBehaviour");
            _prevParent = monoComponent.transform.parent;
            transform.SetParent(monoComponent.transform, true);
        }

        public override void BeforeRemove(Grid grid) => transform.SetParent(_prevParent);

        private GridItem GetDependsOnItem(Vector2Int selfPosition, Grid grid) =>
            grid[selfPosition.x, selfPosition.y - 1].FirstOrNull(AttachToPredicate)?.Item;

        private bool OverlapPredicate(CellRecord r) =>
            !ReferenceEquals(r.Item.Component, this) && r.Cell.IsSolid;

        private bool AttachToPredicate(CellRecord r) =>
            r.Cell.IsSolid && ((r.Cell.Flags & CellFlags.AttachTop) != 0);

        private IEnumerable<Cell> GetCells()
        {
            yield return new Cell(new Vector2Int(0, 0), CellType.Solid, CellFlags.AttachAllSides);
            if (_sphereRequiredAttach)
                yield return new Cell(
                    new Vector2Int(0, -1),
                    CellType.Requirement,
                    CellFlags.AttachTop
                );
        }

        public override IEnumerable<Cell> GetAdditionalCellsAt(Vector2Int position, Grid grid) =>
            null;
    }
}
