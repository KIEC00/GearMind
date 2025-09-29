using System;
using System.Diagnostics;
using Assets.Utils.Runtime;
using UnityEngine;

namespace Assets.Utils.Attributes
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ExponentialViewAttribute : PropertyAttribute
    {
        public readonly double MinValue;
        public readonly double MaxValue;
        public readonly int MinExponent;
        public readonly int MaxExponent;

        public ExponentialViewAttribute(
            double minValue = double.MinValue,
            double maxValue = double.MaxValue,
            int minExponent = -NumericExtensions.DoubleMaxExponent,
            int maxExponent = NumericExtensions.DoubleMaxExponent
        )
        {
            if (minValue > maxValue)
                throw new ArgumentException("minValue must be less than maxValue");
            if (minExponent > maxExponent)
                throw new ArgumentException("minExponent must be less than maxExponent");
            if (minExponent < -NumericExtensions.DoubleMaxExponent)
                throw new ArgumentException(
                    $"minExponent must be greater than {-NumericExtensions.DoubleMaxExponent}"
                );
            if (maxExponent > NumericExtensions.DoubleMaxExponent)
                throw new ArgumentException(
                    $"maxExponent must be less than {NumericExtensions.DoubleMaxExponent}"
                );
            MinValue = minValue;
            MaxValue = maxValue;
            MinExponent = minExponent;
            MaxExponent = maxExponent;
        }
    }
}
