using System.Runtime.CompilerServices;
using UnityEngine;

public class LaserTurnOn : MonoBehaviour, IIncludedObject
{
    [SerializeField] private GameObject Laser;

    public bool IsTurnOn { get; private set; } = false;

    public void TurnOnOff(bool isActive)
    {
        Laser.SetActive(isActive);
        IsTurnOn = isActive;
    }
}
