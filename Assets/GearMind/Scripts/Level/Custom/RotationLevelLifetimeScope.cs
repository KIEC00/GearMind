using Assets.GearMind.Custom.Input;
using Assets.GearMind.Level;
using Assets.GearMind.Scripts.UI;
using Assets.GearMind.UI;
using EditorAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Assets.GearMind.Custom.Level
{
    public class RotationLevelLifetimeScope : LifetimeScope
    {
        [Header("Components")]
        [SerializeField, Required]
        private Transform _environmentAnchor;

        [SerializeField, Required, TypeFilter(typeof(IRotationTarget))]
        private Component _rotationTarget;

        [SerializeField, Required, TypeFilter(typeof(ILevelGoalTrigger))]
        private Component _levelGoalTrigger;

        [SerializeField, Required]
        private NextLevelController _nextLevelController;

        protected override void Configure(IContainerBuilder builder)
        {
            builder
                .RegisterEntryPoint<RotationLevelEntryPoint>()
                .WithParameter(_environmentAnchor)
                .WithParameter((IRotationTarget)_rotationTarget)
                .WithParameter((ILevelGoalTrigger)_levelGoalTrigger);
            builder.Register(LevelContextFactoryMethod, Lifetime.Singleton).AsSelf();

            builder.Register<PauseService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<RotationInputService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<UIManager>(Lifetime.Singleton);
            builder.RegisterComponent(_nextLevelController);
        }

        private static LevelContext LevelContextFactoryMethod(IObjectResolver c)
        {
            var levelProvider = c.Resolve<ILevelProvider>();
            var levelIndex = levelProvider.IndexOf(SceneManager.GetActiveScene().buildIndex);
            return new LevelContext(levelIndex, levelProvider);
        }

        private void OnValidate()
        {
            if (!_environmentAnchor)
                return;

            if (!_levelGoalTrigger)
                _levelGoalTrigger = (Component)
                    _environmentAnchor.GetComponentInChildren<ILevelGoalTrigger>();
            if (!_rotationTarget)
                _rotationTarget = (Component)
                    _environmentAnchor.GetComponentInChildren<IRotationTarget>();
        }
    }
}
