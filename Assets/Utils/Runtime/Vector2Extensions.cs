using UnityEngine;

namespace Assets.Utils.Runtime
{
    public static class Vector2Extensions
    {
        public static Vector2 Abs(this Vector2 vector) =>
            new(Mathf.Abs(vector.x), Mathf.Abs(vector.y));

        public static bool IsOnLine(this Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            return Mathf.Approximately(
                Vector2.SqrMagnitude(point - lineStart) + Vector2.SqrMagnitude(point - lineEnd),
                Vector2.SqrMagnitude(lineStart - lineEnd)
            );
        }

        public static float InverseLerp(this Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            var segmentLength = (lineEnd - lineStart).Abs();
            var iMax = segmentLength.x < segmentLength.y ? 1 : 0;
            return Mathf.InverseLerp(lineStart[iMax], lineEnd[iMax], point[iMax]);
        }

        public static Vector2 Lerp(this Vector2 v, float t) => v.Lerp(Vector2.zero, t);

        public static Vector2 Lerp(this Vector2 v, Vector2 to, float t) => Vector2.Lerp(v, to, t);

        public static Vector2 LerpUnclamped(this Vector2 v, float t) =>
            v.LerpUnclamped(Vector2.zero, t);

        public static Vector2 LerpUnclamped(this Vector2 v, Vector2 to, float t) =>
            Vector2.LerpUnclamped(v, to, t);
    }
}
