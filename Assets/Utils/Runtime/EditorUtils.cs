using UnityEngine;

namespace Assets.Utils.Runtime
{
#if UNITY_EDITOR
    public static class EditorUtils
    {
        public static void SceneText(
            string text,
            Vector3 worldPosition,
            Color textColor = default,
            Vector2 anchor = default,
            float fontSize = 15f
        )
        {
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            if (!view)
                return;

            var screenPosition = view.camera.WorldToScreenPoint(worldPosition);
            var pixelHeight = view.camera.pixelHeight;
            var pixelWidth = view.camera.pixelWidth;
            if (
                screenPosition.y < 0
                || screenPosition.y > pixelHeight
                || screenPosition.x < 0
                || screenPosition.x > pixelWidth
                || screenPosition.z < 0
            )
                return;

            var pixelRatio =
                UnityEditor.HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.right).x
                - UnityEditor.HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.zero).x;

            UnityEditor.Handles.BeginGUI();

            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = (int)fontSize,
                normal = new GUIStyleState() { textColor = textColor },
            };

            var size = style.CalcSize(new GUIContent(text)) * pixelRatio;
            var alignedPosition =
                ((Vector2)screenPosition + size * ((anchor + Vector2.left + Vector2.up) / 2f))
                    * (Vector2.right + Vector2.down)
                + Vector2.up * pixelHeight;

            var rect = new Rect(alignedPosition / pixelRatio, size / pixelRatio);
            GUI.Label(rect, text, style);

            UnityEditor.Handles.EndGUI();
        }
    }
#endif
}
