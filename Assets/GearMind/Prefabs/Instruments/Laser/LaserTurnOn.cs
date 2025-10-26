using UnityEngine;

public class LaserTurnOn : MonoBehaviour, IIncludedObject
{
    [SerializeField] private GameObject Laser;

    public void TurnOnOff(bool isActive)
    {
        Laser.SetActive(isActive);
    }
}
