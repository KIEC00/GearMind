using System;

namespace Assets.Utils.Runtime
{
    public static class NumericExtensions
    {
        public const double DoubleMaxMantissa = 10 - double.Epsilon;
        public const int DoubleMaxExponent = 308;

        public static (double mantissa, int exponent) MantissaAndExponent(this double value)
        {
            var exponent = value.Exponent();
            var mantissa = value / Math.Pow(10, exponent);
            return (mantissa, exponent);
        }

        public static int Exponent(this double value) =>
            value == 0 ? 0 : (int)Math.Log10(Math.Abs(value));

        public static double Mantissa(this double value) => value / Math.Pow(10, value.Exponent());

        private static readonly string[] _suffixes = new[]
        {
            "K",
            "M",
            "B",
            "T",
            "Qa",
            "Qi",
            "Sx",
            "Sp",
            "Oc",
        };

        public static string ToSuffix(this double value, int numbersAfterDot = 0)
        {
            var exponent = value.Exponent();
            if (exponent <= 2)
                return $"{(int)value}";
            var suffixIndex = Math.Min(exponent / 3 - 1, _suffixes.Length - 1);
            var suffix = _suffixes[suffixIndex];
            var number = value / Math.Pow(1000, suffixIndex + 1);
            if (numbersAfterDot <= 0)
                return $"{(int)number}{suffix}";
            var dotScale = (int)Math.Pow(10, numbersAfterDot);
            var scaledValue = (int)(number * dotScale);
            var intPart = scaledValue / dotScale;
            var floatPart = scaledValue % dotScale;
            var numberStr = $"{intPart}.{floatPart.ToString().PadLeft(numbersAfterDot, '0')}";
            return $"{numberStr}{suffix}";
        }

        public const float floatMaxMantissa = 10 - float.Epsilon;
        public const int floatMaxExponent = 38;

        public static int Exponent(this float value) =>
            value == 0 ? 0 : (int)MathF.Log10(MathF.Abs(value));

        public static float Mantissa(this float value) => value / MathF.Pow(10, value.Exponent());

        public static (float mantissa, int exponent) MantissaAndExponent(this float value)
        {
            var exponent = value.Exponent();
            var mantissa = value / MathF.Pow(10, exponent);
            return (mantissa, exponent);
        }
    }
}
