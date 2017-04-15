using StarMap.Cll.Models;
using StarMap.Core.Extensions;
using static StarMap.Cll.Models.Color;

namespace StarMap.Dal.Mappers
{
  // TODO: if color is decide based on some calculations, move this method to the BLL.
  public class Colors
  {
    public static Color MapColor(string type)
    {
      char c = type.IsNullOrEmpty()
        ? default(char) : type[0];
      switch (c)
      {
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
