using Assets.GearMind.Grid.Components;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GridComponent))]
public class GridController : MonoBehaviour
{
    [SerializeField]
    private GridComponent _grid;

    [SerializeField]
    private AbstractGridItemComponent[] _objects;

    private AbstractGridItemComponent _selectedObject;

    private AbstractGridItemComponent _selectedObjectPreview;

    private void SelectObject(AbstractGridItemComponent prefab)
    {
        _selectedObject = prefab;
        if (_selectedObjectPreview != null)
            Destroy(_selectedObjectPreview.gameObject);
        if (_selectedObject == null)
            return;
        _selectedObject.transform.rotation = Quaternion.identity;
        _selectedObjectPreview = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        var physics = _selectedObjectPreview.GetComponentInChildren<Rigidbody2D>();
        if (physics)
            physics.simulated = false;
    }

    private void HandleChangeObject()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            SelectObject(_objects[0]);
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
            SelectObject(_objects[1]);
        else if (Keyboard.current.escapeKey.wasPressedThisFrame)
            SelectObject(null);
    }

    private void HandleMovePreview()
    {
        if (!_selectedObjectPreview)
            return;
        var mouse = Mouse.current.position.ReadValue();
        var mousePlane = _grid.ScreenToPlane(mouse, Camera.main);
        if (!mousePlane.HasValue)
        {
            _selectedObjectPreview.gameObject.SetActive(false);
            return;
        }
        _selectedObjectPreview.gameObject.SetActive(true);
        var cellPos = _grid.ScreenToCell(mouse, Camera.main);
        var position = cellPos.HasValue ? _grid.CellToWorld(cellPos.Value) : mousePlane.Value;

        _selectedObjectPreview.transform.position = position;
        if (cellPos.HasValue && _grid.CanAddItem(_selectedObject, cellPos.Value))
            foreach (var renderer in _selectedObjectPreview.GetComponentsInChildren<Renderer>())
                renderer.material.color = Color.green;
        else
            foreach (var renderer in _selectedObjectPreview.GetComponentsInChildren<Renderer>())
                renderer.material.color = Color.red;
    }

    private void HandleCreate()
    {
        if (!_selectedObject || !Mouse.current.leftButton.wasPressedThisFrame)
            return;
        var mouse = Mouse.current.position.ReadValue();
        var cellPos = _grid.ScreenToCell(mouse, Camera.main);
        if (!cellPos.HasValue)
            return;
        var position = _grid.CellToWorld(cellPos.Value);
        if (!_grid.CanAddItem(_selectedObject, cellPos.Value))
            return;
        var item = Instantiate(_selectedObject);
        item.transform.position = position;

        _grid.AddItem(item, cellPos.Value);
    }

    private void HandleDelete()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame)
            return;
        var mouse = Mouse.current.position.ReadValue();
        var gridPosition = _grid.ScreenToCell(mouse, Camera.main);
        var item = _grid.GetSolidItemAt(gridPosition.Value);
        var removedItems = _grid.RemoveItemsRecursive(item);
        if (removedItems == null)
            return;
        foreach (var removedItem in removedItems)
            Destroy(((AbstractGridItemComponent)removedItem.Component).gameObject);
    }

    private void HandleRotate()
    {
        if (!_selectedObject || !Keyboard.current.rKey.wasPressedThisFrame)
            return;
        (_selectedObject as IRotatable)?.Rotate();
        (_selectedObjectPreview as IRotatable)?.Rotate();
    }

    void Update()
    {
        HandleChangeObject();
        HandleMovePreview();
        HandleCreate();
        HandleDelete();
        HandleRotate();
    }

    void OnValidate()
    {
        if (!_grid)
            _grid = GetComponent<GridComponent>();
    }
}
