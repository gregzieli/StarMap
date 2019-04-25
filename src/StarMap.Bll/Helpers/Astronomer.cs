using StarMap.Cll.Abstractions.Services;
using StarMap.Core.Extensions;
using System;
using Xamarin.Forms;
using static Xamarin.Forms.Color;

namespace StarMap.Bll.Helpers
{
    public class Astronomer : IAstronomer
    {
        // TODO: If there's time, consider calculating the rgb from the b-v color index.
        // The math described here:  https://stackoverflow.com/questions/21977786/star-b-v-color-index-to-apparent-rgb-color
        public Color GetColor(string spectralType)
        {
            if (spectralType.IsNullOrEmpty())
                return Red;

            char c = spectralType[0],
              v = spectralType.Length > 1 ? spectralType[1] : default;
            var i = (int)char.GetNumericValue(v);

            // https://en.wikipedia.org/wiki/Hertzsprung%E2%80%93Russell_diagram
            // https://pl.wikipedia.org/wiki/Plik:Diagram_H-R2_PL.gif <- this one has the most classes
            // http://uofgts.com/Astro/starclass/HRdiag-Big.jpg
            // https://en.wikipedia.org/wiki/Stellar_classification <- this one has the most classes
            switch (c)
            {
                case 'W':
                case 'O':
                    return DodgerBlue;
                case 'B':
                    return i < 4 ? DeepSkyBlue : LightSkyBlue;
                case 'A':
                    return i < 3 ? AliceBlue : White;
                case 'F':
                    if (i < 3)
                        return Ivory;
                    return i < 6 ? LightYellow : LemonChiffon;
                case 'G':
                    return i < 6 ? Yellow : Gold;
                case 'K':
                    if (i < 3)
                        return Orange;
                    return i < 7 ? DarkOrange : OrangeRed;
                case 'M':
                case 'R':
                    return i < 6 ? Tomato : Red;
                case 'N':
                case 'S':
                    return Firebrick;
                default:
                    return OrangeRed;
            }
        }

        // https://en.wikipedia.org/wiki/Color_index
        public double GetTemperature(double colorIndex)
        {
            return Math.Round(((1 / (0.92 * colorIndex + 1.7)) + (1 / (0.92 * colorIndex + 0.62))) * 4600);
        }
    }
}
