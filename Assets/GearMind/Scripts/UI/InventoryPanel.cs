using System;
using System.Collections.Generic;
using Assets.GearMind.Inventory;
using Assets.GearMind.Level;
using EditorAttributes;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.GearMind.UI
{
    public class InventoryPanel : MonoBehaviour, IDisposable
    {
        [SerializeField, Required]
        private CanvasGroup _canvasGroup;

        [SerializeField, Required]
        private ScrollRect _scrollRect;

        [SerializeField, Required]
        private Transform _scrollContent;

        [SerializeField, Required]
        private InventoryItemButton _buttonPrefab;

        private IInventory _inventory;
        private PlacementService _placementService;
        private IIdentityPrefabMap _identityPrefabMap;
        private ILevelStateMachine _stateMachine;

        private Dictionary<IInventoryIdentity, InventoryItemButton> _buttons = new();

        [Inject]
        public void Construct(
            IInventory inventory,
            PlacementService placementService,
            IIdentityPrefabMap identityPrefabMap,
            ILevelStateMachine stateMachine
        )
        {
            _inventory = inventory;
            _inventory.OnChange += HandleInventoryChange;
            _placementService = placementService;
            _identityPrefabMap = identityPrefabMap;
            _stateMachine = stateMachine;
        }

        private void Awake()
        {
            foreach (var kvp in _inventory)
            {
                var button = Instantiate(_buttonPrefab, _scrollContent);
                var identity = kvp.Key;
                var quantity = kvp.Value;

                UpdateButton(button, identity, quantity);
                _buttons.Add(identity, button);
                button.OnPointerDownEvent += () => InstantiateAndStartDragObject(identity);
            }
        }

        private void InstantiateAndStartDragObject(IInventoryIdentity identity)
        {
            if (_stateMachine.CurrentState != LevelState.Edit)
                return;

            if (_inventory[identity] <= 0)
                return;

            var gameObject = _identityPrefabMap[identity];
            if (gameObject == null)
            {
                Debug.LogError($"Inventory item {identity.Name} is not a MonoBehaviour");
                return;
            }

            _scrollRect.enabled = false;
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

        private void UpdateButton(
            InventoryItemButton button,
            IInventoryIdentity identity,
            int quantity
        )
        {
            button.Name = identity.Name;
            button.Quantity = quantity;
            button.Interactable = quantity > 0;
        }

        private void OnDragEnd()
        {
            _placementService.OnDragEnd -= OnDragEnd;
            _scrollRect.enabled = true;
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

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
