using System;
using Assets.Utils.Attributes;
using Assets.Utils.Runtime;
using UnityEditor;
using UnityEngine;

namespace Assets.Utils.Editor
{
    [CustomPropertyDrawer(typeof(ExponentialViewAttribute))]
    public class ExponentialViewDrawer : PropertyDrawer
    {
        private const string TYPE = "double";
        private const float EXP_WIDTH = 40f;
        private const float TEN_WIDTH = 10f;
        private const float X_OFFSET = 5f;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as ExponentialViewAttribute;
            ValidateProperty(property);

            var value = property.doubleValue;
            var (mantissa, exponent) = value.MantissaAndExponent();

            EditorGUI.BeginProperty(rect, label, property);

            rect = EditorGUI.PrefixLabel(rect, label);
            var mantissaRectWidth = rect.width - EXP_WIDTH - TEN_WIDTH - X_OFFSET * 2;
            var mantissaRect = new Rect(rect.x, rect.y, mantissaRectWidth, rect.height);
            var posX = mantissaRect.xMax + X_OFFSET;
            var tenRect = new Rect(posX, rect.y, TEN_WIDTH, rect.height);
            posX += TEN_WIDTH + X_OFFSET;
            var exponentRect = new Rect(posX, rect.y, EXP_WIDTH, rect.height);

            mantissa = EditorGUI.DoubleField(mantissaRect, mantissa);
            mantissa = Math.Clamp(
                mantissa,
                -NumericExtensions.DoubleMaxMantissa,
                NumericExtensions.DoubleMaxMantissa
            );

            EditorGUI.LabelField(tenRect, "E");

            exponent = EditorGUI.IntField(exponentRect, exponent);
            exponent = Math.Clamp(exponent, attr.MinExponent, attr.MaxExponent);
            EditorGUI.EndProperty();

            value = Math.Clamp(mantissa * Math.Pow(10, exponent), attr.MinValue, attr.MaxValue);
            property.doubleValue = value;
            property.serializedObject.ApplyModifiedProperties();
        }

        private void ValidateProperty(SerializedProperty property)
        {
            if (property.type != TYPE)
                throw new ArgumentException($"Property {property.name} is not of type {TYPE}");
        }
    }
}
