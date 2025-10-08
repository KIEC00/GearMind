using System;
using System.Collections;
using System.Linq;
using Assets.GearMind.Grid;
using Assets.GearMind.Grid.Components;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private GridComponent _grid;

    [SerializeField]
    private AbstractGridItemComponent[] _objects;

    private AbstractGridItemComponent _selectedPrefab;

    private AbstractGridItemComponent _flyingObject;

    private bool _isDragging = false;

    private readonly Key[] Keys = Enumerable
        .Range((byte)Key.Digit1, (byte)Key.Digit0)
        .Select(i => (Key)i)
        .ToArray();

    private void HandleChangePrefab()
    {
        for (var i = 0; i < Math.Min(_objects.Length, Keys.Length); i++)
            if (Keyboard.current[Keys[i]].wasPressedThisFrame)
                _selectedPrefab = _objects[i];
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            _selectedPrefab = null;
    }

    private void HandleMoveFlying()
    {
        if (!_flyingObject)
            return;
        var mouse = Mouse.current.position.ReadValue();
        var mousePlane = _grid.ScreenToPlane(mouse, Camera.main);
        if (!mousePlane.HasValue)
        {
            _flyingObject.gameObject.SetActive(false);
            return;
        }
        _flyingObject.gameObject.SetActive(true);
        var cellPos = _grid.ScreenToCell(mouse, Camera.main);
        var position = cellPos.HasValue ? _grid.CellToWorld(cellPos.Value) : mousePlane.Value;

        _flyingObject.transform.position = position;
        if (cellPos.HasValue && _grid.CanAddItem(_flyingObject, cellPos.Value))
            foreach (var renderer in _flyingObject.GetComponentsInChildren<Renderer>())
                renderer.material.color = Color.green;
        else
            foreach (var renderer in _flyingObject.GetComponentsInChildren<Renderer>())
                renderer.material.color = Color.red;
    }

    private void HandleRotate()
    {
        if (!_flyingObject || !Keyboard.current.rKey.wasPressedThisFrame)
            return;
        (_flyingObject as IRotatable)?.Rotate();
    }

    private void PrepareFlying(AbstractGridItemComponent obj)
    {
        var physics = obj.GetComponentInChildren<Rigidbody2D>();
        if (physics)
            physics.simulated = false;
    }

    private void PrepareGrid(AbstractGridItemComponent obj)
    {
        var physics = obj.GetComponentInChildren<Rigidbody2D>();
        if (physics)
            physics.simulated = true;
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
            renderer.material.color = Color.white;
    }

    private bool AddItem(AbstractGridItemComponent item, Vector2Int cellPosition)
    {
        if (!_grid.CanAddItem(item, cellPosition))
            return false;
        if (item.Dynamic)
        {
            _grid.AddItem(item, cellPosition, out var attachedTo);
            var attachTo = attachedTo?.FirstOrDefault()?.Component;
            if (attachTo != null)
                item.transform.SetParent(((MonoBehaviour)attachTo).transform);
        }
        else
            _grid.AddItem(item, cellPosition);
        var position = _grid.CellToWorld(cellPosition);
        item.transform.position = position;
        return true;
    }

    private GridItem GetSolidItemAt(Vector2 screenPosition)
    {
        var cellPos = _grid.ScreenToCell(screenPosition, Camera.main);
        if (!cellPos.HasValue)
            return null;
        return _grid.GetSolidItemAt(cellPos.Value);
    }

    private void HandleStartDrag()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;
        _isDragging = true;
        var mouse = Mouse.current.position.ReadValue();
        var cellItem = GetSolidItemAt(mouse);
        if (cellItem != null)
        {
            var removedItems = _grid.RemoveItemsRecursive(cellItem);
            if (removedItems == null)
                return;
            foreach (var removedItem in removedItems)
                if (removedItem != cellItem)
                    Destroy(((AbstractGridItemComponent)removedItem.Component).gameObject);
            _flyingObject = (AbstractGridItemComponent)cellItem.Component;
            PrepareFlying(_flyingObject);
        }
        else if (_selectedPrefab)
        {
            _flyingObject = Instantiate(_selectedPrefab);
            PrepareFlying(_flyingObject);
        }
        else
            _isDragging = false;
    }

    private void HandleEndDrag()
    {
        if (!_isDragging || !Mouse.current.leftButton.wasReleasedThisFrame)
            return;
        _isDragging = false;
        var mouse = Mouse.current.position.ReadValue();
        var cellPos = _grid.ScreenToCell(mouse, Camera.main);
        if (cellPos.HasValue && AddItem(_flyingObject, cellPos.Value))
        {
            PrepareGrid(_flyingObject);
            AddItem(_flyingObject, cellPos.Value);
        }
        else
            Destroy(_flyingObject.gameObject);
        _flyingObject = null;
    }

    void Update()
    {
        HandleChangePrefab();
        HandleStartDrag();
        HandleRotate();
        HandleMoveFlying();
        HandleEndDrag();
    }

    void OnValidate()
    {
        if (!_grid)
            _grid = GetComponent<GridComponent>();
    }
}
