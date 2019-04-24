using StarMap.Core.Extensions;
using Xamarin.Forms;
using static Xamarin.Forms.Color;

namespace StarMap.Dal.Mappers
{
    public class Colors
    {
        public static Color MapColor(string type)
        {
            switch (type.IsNullOrEmpty() ? default : type[0])
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
