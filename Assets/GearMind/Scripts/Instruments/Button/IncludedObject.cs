using System;
using UnityEngine;

public class IncludedObject : MonoBehaviour
{
    [SerializeField] private GameObject IncludeObject;
    private IIncludedObject include;
    

    public void TurnOnOff(bool isActive)
    {
        include.TurnOnOff(isActive);
    }

    public void Awake()
    {
        include = IncludeObject.GetComponent<IIncludedObject>();
    }


}
