using System;
using UnityEngine;

public interface IIncludedObject 
{
    public void TurnOnOff(bool isActive);
    public bool IsTurnOn { get; }
    
    
}
