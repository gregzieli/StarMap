using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarMap.Core.Utils
{
  public static class Normalizer
  {
    /// <summary>
    /// Normalizes a value to the standard [0, 1] range.
    /// </summary>
    /// <param name="x">Value to normalize</param>
    /// <param name="xMin">Source range minimum</param>
    /// <param name="xMax">Source range maximum</param>
    public static double Normalize(double x, double xMin, double xMax)
      => (x - xMin) / (xMax - xMin);

    /// <summary>
    /// Normalizes a value to a custom [minRange, maxRange] range.
    /// </summary>
    /// <param name="x">Value to normalize</param>
    /// <param name="xMin">Source range minimum</param>
    /// <param name="xMax">Source range maximum</param>
    /// <param name="minRange">Output range minimum</param>
    /// <param name="maxRange">Output range maximum</param>
    /// <returns></returns>
    public static double Normalize(double x, double xMin, double xMax, double minRange, double maxRange)
    {
      double customRange = maxRange - minRange;
      var standard = Normalize(x, xMin, xMax);
      //https://stackoverflow.com/questions/10364575/normalization-in-variable-range-x-y-in-matlab
      return standard * customRange + minRange;
    }

    public static IEnumerable<double> Normalize(this IEnumerable<double> values)
    {
      double min = values.Min(),
        max = values.Max(),
        range = max - min;

      foreach (var v in values)
        yield return (v - min) / range;
    }

    public static IEnumerable<double> Normalize(this IEnumerable<double> values, double minRange, double maxRange)
    {
      double min = values.Min(),
        max = values.Max(),
        range = max - min,
        customRange = maxRange - minRange;

      foreach (var v in values)
      {
        var standard = (v - min) / range;
        yield return standard * customRange + minRange;

      }
    }
  }
}
