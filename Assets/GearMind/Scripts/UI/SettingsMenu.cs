using UnityEngine;

namespace Assets.GearMind.Scripts.UI
{
    public class SettingsMenu : MonoBehaviour
    {
        public void CloseMenu()
        {
            gameObject.SetActive(false);
        }
    }
}