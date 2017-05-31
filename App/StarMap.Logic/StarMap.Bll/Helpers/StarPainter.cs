using StarMap.Cll.Abstractions.Services;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Extensions;
using Xamarin.Forms;
using static Xamarin.Forms.Color;

namespace StarMap.Bll.Helpers
{
  public class StarPainter : IStarPainter
  {
    public Color GetColor(StarDetail star)
    {
      var type = star.SpectralType;
      char c = type.IsNullOrEmpty()
        ? default(char) : type[0];
      switch (c)
      {
        // From CELESTIA, there are: a, b (light blue), f (beige), g, o (dark blue) 
        // TODO: use luminosity and/or some other params for light/dark tones.
        //       eventually, use maybe own enum, not xamarin's colors.
        case 'A':
          return White;
        case 'B':
          return Blue;
        case 'F':
        case 'G':
          return Yellow;
        case 'K':
          return Orange;
        case 'M':
          return Red;
        default:
          return Yellow;
      }
    }
  }
}
