using UnityEngine;

namespace Assets.Utils.Runtime
{
    public static class RectExtensions
    {
        public static Rect ToRect(this Bounds bounds)
        {
            var center2D = (Vector2)bounds.center;
            var size2D = (Vector2)bounds.size;
            var min = center2D - size2D * 0.5f;
            return new Rect(min, size2D);
        }

        public static bool Contains(this Rect outer, Rect inner) =>
            outer.xMin <= inner.xMin
            && outer.yMin <= inner.yMin
            && outer.xMax >= inner.xMax
            && outer.yMax >= inner.yMax;
    }
}
