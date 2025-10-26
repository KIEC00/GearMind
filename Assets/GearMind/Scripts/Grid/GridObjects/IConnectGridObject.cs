using System;
using UnityEngine;
using UnityEngine.Events;

public interface IConnectGridObject 
{
    public event Action OnDestroyConnectObject;
    public void DestroyObject();
}
