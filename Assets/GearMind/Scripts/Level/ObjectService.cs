using System;
using Assets.GearMind.Instruments;
using Assets.GearMind.Inventory;
using Assets.GearMind.State;
using UnityEngine;

namespace Assets.GearMind.Level
{
    public class ObjectService : IObjectService
    {
        private readonly IInventory _inventory;
        private readonly IGameplayObjectService _gameplayObjectService;
        private readonly IStateService _stateService;

        public ObjectService(
            IInventory inventory,
            IGameplayObjectService gameplayObjectService,
            IStateService stateService
        )
        {
            _inventory = inventory;
            _gameplayObjectService = gameplayObjectService;
            _stateService = stateService;
        }

        public GameObject InstantiateObject(GameObject prefab)
        {
            if (!prefab)
                throw new ArgumentNullException(nameof(prefab));

            var instance = UnityEngine.Object.Instantiate(prefab);

            if (instance.TryGetComponent<InventoryIdentityComponent>(out var inventoryComponent))
                _inventory[inventoryComponent.InventoryIdentity] -= 1;
            if (instance.TryGetComponent<IGameplayObject>(out var gameplayObject))
                _gameplayObjectService.Register(gameplayObject);
            if (instance.TryGetComponent<IHaveState>(out var stateComponent))
                _stateService.Register(stateComponent, saveState: false);

            return instance;
        }

        public void DestroyObject(GameObject instance)
        {
            if (!instance)
                throw new ArgumentNullException(nameof(instance));

            if (instance.TryGetComponent<InventoryIdentityComponent>(out var inventoryComponent))
                _inventory[inventoryComponent.InventoryIdentity] += 1;
            if (instance.TryGetComponent<IGameplayObject>(out var gameplayObject))
                _gameplayObjectService.Unregister(gameplayObject);
            if (instance.TryGetComponent<IHaveState>(out var stateComponent))
                _stateService.Unregister(stateComponent);

            UnityEngine.Object.Destroy(instance);
        }
    }
}
