using Assets.GearMind.Scripts.UI.Screens;
using R3;
using VContainer;

namespace Assets.GearMind.Scripts.UI.Game
{
    public class GameplayUIManager : UIManager
    {
        private readonly IObjectResolver _resolver;
        

        public GameplayUIManager(IObjectResolver resolver) : base(resolver)
        {
            _resolver = resolver;
            
        }

        public PauseMenuViewModel OpenPauseMenuGameplay()
        {
            var viewModel = _resolver.Resolve<PauseMenuViewModel>(new object[] { this });
            var rootUI = _resolver.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenScreen(viewModel);

            return viewModel;
        }

        public SettingsViewModel OpenSettings()
        {
            var a = _resolver.Resolve<SettingsViewModel>();
            var rootUI = _resolver.Resolve<UIGameplayRootViewModel>();

            rootUI.OpenPopup(a);

            return a;
        }
    }

}
