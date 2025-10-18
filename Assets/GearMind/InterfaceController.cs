using Assets.GearMind.Grid.Tests;
using UnityEngine;

namespace Assets.GearMind
{
    public class InterfaceController : MonoBehaviour
    {
        [SerializeField]
        private GridController gridController;

        public void OnObject1ButtonClick()
        {
            gridController.StartPlacingObjectByIndex(0);
        }

        public void OnObject2ButtonClick()
        {
            gridController.StartPlacingObjectByIndex(1);
        }
    }
}
