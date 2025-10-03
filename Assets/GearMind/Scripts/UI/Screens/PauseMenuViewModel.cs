using System;
using Assets.GearMind.Scripts.UI.Game;

namespace Assets.GearMind.Scripts.UI.Screens
{
    public class PauseMenuViewModel : WindowViewModel
    {
        private readonly GameplayUIManager _uiManager;
        public override string Id => "PauseMenu";
        public PauseMenuViewModel(GameplayUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public void RequestOpenSettings()
        {
            _uiManager.OpenSettings();
        }

    }
}