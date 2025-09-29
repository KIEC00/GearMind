using System;
using System.Collections.Generic;

namespace Assets.Utils.Runtime
{
    public static class RandomUtils
    {
        public static IEnumerable<int> RandomInts(int minValue = 0, int maxValue = int.MaxValue) =>
            RandomInts(new Random(), minValue, maxValue);

        public static IEnumerable<int> RandomInts(
            Random rnd,
            int minValue = 0,
            int maxValue = int.MaxValue
        )
        {
            while (true)
                yield return rnd.Next(minValue, maxValue);
        }
    }
}
