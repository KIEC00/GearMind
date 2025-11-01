using System;
using UnityEngine;
using UnityEngine.Events;

public class ConnectObject : MonoBehaviour, IConnectGridObject
{
    public event Action OnDestroyConnectObject;

    public void DestroyObject()
    {
        OnDestroyConnectObject?.Invoke();
        Destroy(transform.parent.gameObject);
    }
}
