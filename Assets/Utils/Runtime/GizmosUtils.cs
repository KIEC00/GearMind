using UnityEngine;

namespace Assets.Utils.Runtime
{
    public static class GizmosUtils
    {
        public static void DrawWireCube(Vector3 position, Vector3 size, Quaternion rotation)
        {
            var oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(position, rotation, size);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = oldMatrix;
        }
    }
}
