using Assets.GearMind.Scripts.UI.Game;
using UnityEngine;
using VContainer;

namespace Assets.GearMind.Scripts.UI
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIRootView _uiRootPrefab;  
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab; 

        [Inject] private IObjectResolver _resolver; 

        private void Start()
        {
            InitUI();
        }

        private void InitUI()
        {
            var uiRoot = FindFirstObjectByType<UIRootView>();
            if (uiRoot == null)
            {
                uiRoot = Instantiate(_uiRootPrefab);
            }

            //UI для сцены
            var uiSceneRootBinder = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiSceneRootBinder.gameObject);

            var uiSceneRootViewModel = _resolver.Resolve<UIGameplayRootViewModel>();
            uiSceneRootBinder.Bind(uiSceneRootViewModel);

            // Открываем экран через UIManager
            var uiManager = _resolver.Resolve<GameplayUIManager>();
            uiManager.OpenPauseMenuGameplay();
        }
    }
}

