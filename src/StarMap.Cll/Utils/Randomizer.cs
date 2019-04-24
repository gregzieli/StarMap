using System;
using System.Linq;

namespace StarMap.Core.Utils
{
    public static class Randomizer
    {
        private static readonly Random Random = new Random();
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string RandomString(int length)
          => new string(Enumerable.Repeat(Chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());


        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="minVal">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxVal">The INCLUSIVE upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less OR EQUAL TO maxValue.</returns>
        public static int RandomInt(int minVal, int maxVal)
          => Random.Next(minVal, maxVal + 1);
    }
}