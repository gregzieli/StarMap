using StarMap.Core.Extensions;

namespace StarMap.Cll.Models.Cosmos
{
  public class Star : StarBase
  {
    public int? ConstellationId { get; set; }

    /// <summary>
    /// Gets or sets the star's brightness as visible from Earth.
    /// </summary>
    public double ApparentMagnitude { get; set; }

    public double AbsoluteMagnitude { get; set; }

    public double RightAscension { get; set; }

    public double Declination { get; set; }

    public double RightAscensionRad { get; set; } // Probably TBR

    public double DeclinationRad { get; set; } // Probably TBR

    public double X { get; set; }

    public double Y { get; set; }

    public double Z { get; set; }

    public string Designation => Name ?? (!Flamsteed.IsNullOrEmpty() ? $"{Flamsteed} {Bayer}" : Bayer);
  }
}
