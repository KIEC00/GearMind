using System.Collections.Generic;
using Assets.GearMind.Grid;
using Assets.GearMind.Grid.Components;
using UnityEngine;

public class TestGridItem : AbstractGridItemComponent, IRotatable
{
    public static int Index = 0;

    [SerializeField]
    private bool _sphereRequiredAttachTop = false;

    [SerializeField]
    private bool _dynamic = false;
    public override bool Dynamic => _dynamic;

    [SerializeField]
    private GameObject _sphere;

    public override IEnumerable<Cell> Cells => GetCells();

    public void Rotate(RotationDirection direction)
    {
        transform.Rotate(new Vector3(0, 0, 360 + (sbyte)direction * 90));
    }

    public void Start()
    {
        _sphere.SetActive(_sphereRequiredAttachTop);
    }

    private IEnumerable<Cell> GetCells()
    {
        yield return new Cell(new Vector2Int(0, 0), CellFlags.Solid | CellFlags.AttachableAll);
        if (!_sphereRequiredAttachTop)
            yield break;
        var rotation = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        yield return new Cell(
            new Vector2Int((int)Mathf.Sin(rotation), -(int)Mathf.Cos(rotation)),
            CellFlags.RequireAttachTop
        );
    }
}
