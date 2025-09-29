using System;
using Assets.Utils.Runtime;
using UnityEditor;
using UnityEngine;

namespace Assets.Utils.Editor
{
    [CustomPropertyDrawer(typeof(SerializableGuid))]
    public class SerializableGuidDrawer : PropertyDrawer
    {
        private const float X_OFFSET = 5f;
        private const float Y_OFFSET = 5f;
        private static readonly string[] BUTTONS = new string[] { "Copy", "Paste", "New" };
        private static readonly string PROPERTY_NAME = "_serialized";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var serializedGuid = property.FindPropertyRelative(PROPERTY_NAME);
            var serialized = serializedGuid.stringValue;

            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, label);
            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.TextField(position, serialized);
            position.y += EditorGUIUtility.singleLineHeight + Y_OFFSET;

            var buttonWidth = (position.width - X_OFFSET * BUTTONS.Length) / BUTTONS.Length;
            position.width = buttonWidth;
            var prevEnabled = GUI.enabled;
            GUI.enabled = true; // Force enable
            if (GUI.Button(position, BUTTONS[0]))
                EditorGUIUtility.systemCopyBuffer = serialized;
            GUI.enabled = prevEnabled;
            position.x += buttonWidth + X_OFFSET;
            if (GUI.Button(position, BUTTONS[1]))
                serializedGuid.stringValue = EditorGUIUtility.systemCopyBuffer;
            position.x += buttonWidth + X_OFFSET;
            if (GUI.Button(position, BUTTONS[2]))
                serializedGuid.stringValue = $"{Guid.NewGuid()}";

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            EditorGUIUtility.singleLineHeight * 2 + Y_OFFSET * BUTTONS.Length;
    }
}
