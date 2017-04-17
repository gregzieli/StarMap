using System;
using static StarMap.Cll.Models.Geolocation.CardinalPoint;
using static StarMap.Cll.Models.Geolocation.CoordinateType;

namespace StarMap.Cll.Models.Geolocation
{
  public class DegreeCoordinate
  {
    private double _value;
    private CoordinateType _type;

    public DegreeCoordinate(double value, CoordinateType type)
    {
      _value = value;
      _type = type;
      Convert();
    }

    public int Degrees { get; set; }

    public int Minutes { get; set; }

    public int Seconds { get; set; }    

    CardinalPoint GetCardinalPoint()
    {
      switch (_type)
      {
        case Latitude:
          return _value < 0
            ? S : N;
        case Longitude:
          return _value < 0
            ? W : E;
        default:
          throw new NotSupportedException("There are only two types of coordinates");
      }
    }

    //https://en.wikipedia.org/wiki/Geographic_coordinate_conversion#Conversion_from_Decimal_Degree_to_DMS
    void Convert()
    {
      var absoluteValue = Math.Abs(_value);
      Degrees = (int)absoluteValue;
      var delta = absoluteValue - Degrees;
      Minutes = (int)(delta * 60);
      Seconds = (int)Math.Round(3600 * delta - 60 * Minutes, MidpointRounding.AwayFromZero);
    }
    
    public override string ToString()
    {
      return $"{Degrees}\u00B0{Minutes.ToString("D2")}'{Seconds.ToString("D2")}\"{GetCardinalPoint()}";
    }
  }

  enum CardinalPoint
  {
    N,

    S,
    
    E,

    W
  }
}
