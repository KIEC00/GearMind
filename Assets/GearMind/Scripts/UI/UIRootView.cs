using UnityEngine;

namespace Assets.GearMind.Scripts.UI
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private Transform _uiSceneContainer;


        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneUI();
            
            sceneUI.transform.SetParent(_uiSceneContainer, false);
        }

        private void ClearSceneUI()
        {
            var childCount = _uiSceneContainer.childCount;
            for (var i = 0; i < childCount; i++)
            {
                Destroy(_uiSceneContainer.GetChild(i).gameObject);
            }
        }
    }
}