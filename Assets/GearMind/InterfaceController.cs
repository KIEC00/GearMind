using UnityEngine;

namespace Assets.GearMind
{
    public class InterfaceController : MonoBehaviour
    {
        [SerializeField] private GridController gridController;
        [SerializeField] private SimulationManager _simulationManager;
        
        public void OnObject1ButtonClick()
        {
            if (IsSimulationActive()) return;
            
            gridController.StartPlacingObjectByIndex(0);
        }
        
        public void OnObject2ButtonClick()
        {
            if (IsSimulationActive()) return;

            gridController.StartPlacingObjectByIndex(1);
        }
        private bool IsSimulationActive()
        {
            return _simulationManager != null && _simulationManager.IsSimulating;
        }
    }
}