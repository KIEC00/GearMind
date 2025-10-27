using System;
using System.Collections.Generic;
using Assets.GearMind.Inventory;
using Assets.GearMind.Objects;
using UnityEngine;

namespace Assets.GearMind.Level
{
    public class ObjectService : IObjectService, IDisposable
    {
        private readonly IInventory _inventory;
        private readonly PlacementService _placementService;

        private readonly HashSet<IGameplayObject> _gameplayObjects = new();

        public ObjectService(IInventory inventory, PlacementService placementService)
        {
            _inventory = inventory;
            _placementService = placementService;
            _placementService.DestroyRequest += DestroyObject;
        }

        public void InstantiateObject(GameObject prefab)
        {
            if (!prefab)
                throw new ArgumentNullException(nameof(prefab));
            var instance = UnityEngine.Object.Instantiate(prefab);
            if (!instance.TryGetComponent<InventoryIdentityComponent>(out var inventoryComponent))
                Debug.LogError("Object must have InventoryIdentityComponent", instance);
            if (!instance.TryGetComponent<IDragAndDropTarget>(out var dragTarget))
                Debug.LogError("Object must have IDragAndDropTarget", instance);
            if (inventoryComponent == null || dragTarget == null)
            {
                UnityEngine.Object.Destroy(instance);
                return;
            }
            if (instance.TryGetComponent<IGameplayObject>(out var gameplayObject))
            {
                RegisterGameplayObject(gameplayObject);
                gameplayObject.EnterEditMode();
            }
            if (inventoryComponent)
                _inventory[inventoryComponent.InventoryIdentity] -= 1;
            _placementService.StartDragObject(instance);
        }

        public void DestroyObject(GameObject instance)
        {
            if (!instance)
                throw new ArgumentNullException(nameof(instance));
            if (!instance.TryGetComponent<InventoryIdentityComponent>(out var inventoryComponent))
                Debug.LogError("Object must have InventoryIdentityComponent", instance);
            if (instance.TryGetComponent<IGameplayObject>(out var gameplayObject))
                UnregisterGameplayObject(gameplayObject);
            if (inventoryComponent)
                _inventory[inventoryComponent.InventoryIdentity] += 1;
            UnityEngine.Object.Destroy(instance);
        }

        public void RegisterGameplayObject(IGameplayObject gameplayObject) =>
            _gameplayObjects.Add(gameplayObject);

        public void UnregisterGameplayObject(IGameplayObject gameplayObject) =>
            _gameplayObjects.Remove(gameplayObject);

        public void EnterEditMode()
        {
            foreach (var go in _gameplayObjects)
                go.EnterEditMode();
        }

        public void EnterSimulationMode()
        {
            foreach (var go in _gameplayObjects)
                go.EnterPlayMode();
        }

        public void Dispose() => _placementService.DestroyRequest -= DestroyObject;
    }
}
