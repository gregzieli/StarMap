namespace StarMap.Core.Extensions
{
    public static partial class NumberExtensions
    {
        /// <summary>
        /// Normalizes a value within its [0, <paramref name="xMax"/>] range to the standard [0, 1] range.
        /// </summary>
        /// <param name="x">Value to normalize</param>
        /// <param name="xMax">Source range maximum</param>
        public static float Normalize(this int x, float xMax)
          => Normalize(x, 0, xMax);

        /// <summary>
        /// Normalizes a value within its [<paramref name="xMin"/>, <paramref name="xMax"/>] range to the standard [0, 1] range.
        /// </summary>
        /// <param name="x">Value to normalize</param>
        /// <param name="xMin">Source range minimum</param>
        /// <param name="xMax">Source range maximum</param>
        public static float Normalize(this int x, float xMin, float xMax)
          => (x - xMin) / (xMax - xMin);

        /// <summary>
        /// Normalizes a value within its [<paramref name="xMin"/>, <paramref name="xMax"/>] range 
        /// to a custom [<paramref name="minRange"/>, <paramref name="maxRange"/>] range.
        /// </summary>
        /// <param name="x">Value to normalize</param>
        /// <param name="xMin">Source range minimum</param>
        /// <param name="xMax">Source range maximum</param>
        /// <param name="minRange">Output range minimum</param>
        /// <param name="maxRange">Output range maximum</param>
        /// <returns></returns>
        public static float Normalize(this int x, float xMin, float xMax, float minRange, float maxRange)
        {
            var customRange = maxRange - minRange;
            var standard = Normalize(x, xMin, xMax);
            //https://stackoverflow.com/questions/10364575/normalization-in-variable-range-x-y-in-matlab
            return standard * customRange + minRange;
        }
    }
}
