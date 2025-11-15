using System.Collections.Generic;
using System.Linq;
using Assets.GearMind.Common;
using Assets.GearMind.Grid;
using Assets.GearMind.Input;
using Assets.GearMind.Instruments;
using Assets.GearMind.Inventory;
using Assets.GearMind.Level.States;
using Assets.GearMind.Scripts.UI;
using Assets.GearMind.State;
using Assets.GearMind.UI;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Assets.GearMind.Level
{
    public class LevelLifetimeScope : LifetimeScope
    {
        [Header("Configuration")]
        [SerializeField, Required]
        private InventoryFactorySO _inventoryFactory;

        [SerializeField]
        private LayerMask _dragLayers;

        [Header("Components")]
        [SerializeField, Required]
        private Transform _environmentAnchor;

        [SerializeField, Required]
        private GraphicRaycaster _graphicRaycaster;

        [SerializeField, Required]
        private EventSystem _eventSystem;

        [SerializeField, Required]
        private GridComponent _grid;

        [SerializeField, Required]
        private NextLevelController _nextLevelController;

        [SerializeField, Required, TypeFilter(typeof(ILevelGoalTrigger))]
        private Component _levelGoalTrigger;

        protected override void Configure(IContainerBuilder builder)
        {
            builder
                .RegisterEntryPoint<LevelEntryPoint>(Lifetime.Singleton)
                .WithParameter(_environmentAnchor);
            builder.Register(LevelContextFactoryMethod, Lifetime.Singleton).AsSelf();

            builder.RegisterInstance(_inventoryFactory.CreateInventory()).As<IInventory>();
            builder.RegisterInstance(CreateIdentityPrefabMap(_inventoryFactory));

            builder.RegisterInstance(_graphicRaycaster);
            builder.RegisterInstance(_eventSystem);
            builder.RegisterInstance(_grid);

            builder.Register<PauseService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InputService>(Lifetime.Transient).AsImplementedInterfaces();
            builder.Register<CameraProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ScreenRaycaster>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<LevelEditState>(Lifetime.Singleton).AsSelf();
            builder.Register<LevelSimulationState>(Lifetime.Singleton).AsSelf();

            builder.Register<ObjectService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<StateService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameplayObjectService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder
                .Register<PlacementService>(Lifetime.Singleton)
                .WithParameter("objectsParent", _environmentAnchor)
                .WithParameter("layerMask", _dragLayers)
                .All();

            builder.Register(LevelStateMachineFactoryMethod, Lifetime.Singleton).All();

            builder.Register<UIManager>(Lifetime.Singleton);
            builder.RegisterComponent(_nextLevelController);
            builder.RegisterComponent(_levelGoalTrigger).AsImplementedInterfaces();
        }

        private static LevelStateMachine LevelStateMachineFactoryMethod(IObjectResolver c) =>
            new LevelStateMachine()
                .RegisterState(LevelState.Edit, c.Resolve<LevelEditState>())
                .RegisterState(LevelState.Simulate, c.Resolve<LevelSimulationState>());

        private static IIdentityPrefabMap CreateIdentityPrefabMap(
            InventoryFactorySO inventoryFactory
        ) =>
            new IdentityPrefabMap(
                inventoryFactory
                    .GetComponents()
                    .Select(component => new KeyValuePair<IInventoryIdentity, GameObject>(
                        component.InventoryIdentity,
                        component.gameObject
                    ))
            );

        private static LevelContext LevelContextFactoryMethod(IObjectResolver c)
        {
            var levelProvider = c.Resolve<ILevelProvider>();
            var levelIndex = levelProvider.IndexOf(SceneManager.GetActiveScene().buildIndex);
            return new LevelContext(levelIndex, levelProvider);
        }

        private void OnValidate()
        {
            if (!_levelGoalTrigger && _environmentAnchor)
                _environmentAnchor.GetComponentInChildren<ILevelGoalTrigger>();
        }
    }
}
