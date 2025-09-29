using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Utils.Runtime
{
    public static class GameObjectExtensions
    {
        public static void Activate(this GameObject gameObject) => gameObject.SetActive(true);

        public static void Deactivate(this GameObject gameObject) => gameObject.SetActive(false);

        public static bool IsActive(this GameObject gameObject) => gameObject.activeSelf;

        public static IEnumerable<T> WhereNotUnityNull<T>(this IEnumerable<T> items) =>
            items.Where(item => item.IsUnityNull());
    }
}
