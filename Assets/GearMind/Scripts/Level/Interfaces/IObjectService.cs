using UnityEngine;

namespace Assets.GearMind.Level
{
    public interface IObjectService
    {
        GameObject InstantiateObject(GameObject prefab);
        void DestroyObject(GameObject instance);
    }
}
