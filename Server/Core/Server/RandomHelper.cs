using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Core.Server
{
    public static class RandomHelper
    {
        private static Random _random = new Random();

        public static double CalculateProbability(int percentage, int iterations)
        {
            var eventOccurredCount = 0;
            const int maxRandomValue = 100;
            for (int i = 0; i < iterations; i++)
            {
                eventOccurredCount += _random.Next(1, maxRandomValue + 1) <= percentage ? 1 : 0;
            }
            return (double)eventOccurredCount / iterations;
        }

        public static int NextInt(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}
