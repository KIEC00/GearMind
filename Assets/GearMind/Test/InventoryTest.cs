using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GearMind.Inventory;
using Assets.GearMind.Level;
using Assets.GearMind.Test;
using EditorAttributes;
using UnityEngine;
using VContainer;

public class InventoryTest : MonoBehaviour, IDisposable
{
    [SerializeField, Required]
    private CanvasGroup _canvasGroup;

    private IInventory _inventory;
    private PlacementService _placementService;
    private IIdentityPrefabMap _identityPrefabMap;

    private Dictionary<IInventoryIdentity, InventoryTestButton> _buttons = new();

    [Inject]
    public void Construct(
        IInventory inventory,
        PlacementService placementService,
        IIdentityPrefabMap identityPrefabMap
    )
    {
        _inventory = inventory;
        _inventory.OnChange += HandleInventoryChange;
        _placementService = placementService;
        _identityPrefabMap = identityPrefabMap;
    }

    private void Awake()
    {
        var buttons = GetComponentsInChildren<InventoryTestButton>();
        foreach (var (button, item) in buttons.Zip(_inventory, (button, item) => (button, item)))
        {
            var identity = item.Key;
            var quantity = item.Value;
            UpdateButton(button, identity, quantity);
            _buttons.Add(identity, button);
            button.OnPointerDownEvent += () => InstantiateAndStartDragObject(identity);
        }
    }

    private void InstantiateAndStartDragObject(IInventoryIdentity identity)
    {
        var gameObject = _identityPrefabMap[identity];
        if (gameObject == null)
        {
            Debug.LogError($"Inventory item {identity.Name} is not a MonoBehaviour");
            return;
        }
        _placementService.OnDragEnd += OnDragEnd;
        _placementService.InstantiateAndStartDragObject(gameObject);
        SetGhostState();
    }

    private void HandleInventoryChange(InventoryChangeEventData data)
    {
        var button = _buttons.GetValueOrDefault(data.Identity);
        if (button == null)
            return;
        UpdateButton(button, data.Identity, data.CurrentCount);
    }

    private void UpdateButton(InventoryTestButton button, IInventoryIdentity identity, int quantity)
    {
        button.Name = identity.Name;
        button.Quantity = quantity;
    }

    private void OnDragEnd()
    {
        _placementService.OnDragEnd -= OnDragEnd;
        SetDefaultState();
    }

    private void SetDefaultState()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
    }

    private void SetGhostState()
    {
        _canvasGroup.alpha = 0.5f;
        _canvasGroup.interactable = false;
    }

    public void Dispose()
    {
        _inventory.OnChange -= HandleInventoryChange;
        _placementService.OnDragEnd -= OnDragEnd;
    }
}
