using System.Collections.Generic;
using System.Linq;

namespace StarMap.Core.Extensions
{
  public static partial class NumberExtensions
  {
    /// <summary>
    /// Normalizes a value within its [0, <paramref name="xMax"/>] range to the standard [0, 1] range.
    /// </summary>
    /// <param name="x">Value to normalize</param>
    /// <param name="xMax">Source range maximum</param>
    public static double Normalize(this double x, double xMax)
      => Normalize(x, 0, xMax);

    /// <summary>
    /// Normalizes a value within its [<paramref name="xMin"/>, <paramref name="xMax"/>] range to the standard [0, 1] range.
    /// </summary>
    /// <param name="x">Value to normalize</param>
    /// <param name="xMin">Source range minimum</param>
    /// <param name="xMax">Source range maximum</param>
    public static double Normalize(this double x, double xMin, double xMax)
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
    public static double Normalize(this double x, double xMin, double xMax, double minRange, double maxRange)
    {
      double customRange = maxRange - minRange;
      var standard = Normalize(x, xMin, xMax);
      //https://stackoverflow.com/questions/10364575/normalization-in-variable-range-x-y-in-matlab
      return standard * customRange + minRange;
    }

    /// <summary>
    /// Normalizes all values to the standard [0, 1] range.
    /// </summary>
    /// <returns>A new, normalized collection.</returns>
    public static IEnumerable<double> Normalize(this IEnumerable<double> values)
    {
      double min = values.Min(),
        max = values.Max();

      foreach (var v in values)
        yield return v.Normalize(min, max);
    }

    /// <summary>
    /// Normalizes all values to a custom [<paramref name="minRange"/>, <paramref name="maxRange"/>] range.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="minRange"></param>
    /// <param name="maxRange"></param>
    /// <returns></returns>
    public static IEnumerable<double> Normalize(this IEnumerable<double> values, double minRange, double maxRange)
    {
      double min = values.Min(),
        max = values.Max();

      foreach (var v in values)
        yield return v.Normalize(min, max, minRange, maxRange);
    }
  }
}
