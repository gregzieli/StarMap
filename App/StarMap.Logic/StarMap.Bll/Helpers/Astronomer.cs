using StarMap.Cll.Abstractions.Services;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Extensions;
using System;
using Xamarin.Forms;
using static Xamarin.Forms.Color;

namespace StarMap.Bll.Helpers
{
  public class Astronomer : IAstronomer
  {
    public Color GetColor(string spectralType)
    {
      if (spectralType.IsNullOrEmpty())
        return Red;

      char c = spectralType[0],
        v = spectralType.Length > 1 ? spectralType[1] : default(char);
      int i = (int)Char.GetNumericValue(v);

      // https://en.wikipedia.org/wiki/Hertzsprung%E2%80%93Russell_diagram
      // https://pl.wikipedia.org/wiki/Plik:Diagram_H-R2_PL.gif <- this one has the most classes
      // http://uofgts.com/Astro/starclass/HRdiag-Big.jpg
      switch (c)
      {      
        case 'W':
          return DarkBlue;
        case 'O':
          return DarkBlue;
        case 'B':
          return i < 3 ? DarkBlue : Blue;
        case 'A':
          return White;
        case 'F':
          return i < 4 ? White : Yellow;
        case 'G':
          return Yellow;
        case 'K':
          return Orange;
        case 'M':
        case 'R':
          return Red;
        case 'N':
        case 'S':
          return DarkRed;
        default:
          return Red;
      }
    }

    // https://en.wikipedia.org/wiki/Color_index
    public double GetTemperature(double colorIndex)
    {
      return Math.Round(((1 / (0.92 * colorIndex + 1.7)) + (1 / (0.92 * colorIndex + 0.62))) * 4600);
    }
  }
}
