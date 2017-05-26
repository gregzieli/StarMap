using Xamarin.Forms;

namespace StarMap.Cll.Models.Cosmos
{
  public class StarDetail : StarBase
  {

    public int? HipparcosId { get; set; }

    public int? HenryDraperId { get; set; }

    public string GlieseId { get; set; }

    /// <summary>
    /// Gets or sets the name of the multi-star system.
    /// </summary>
    public string Base { get; set; }

    public Constellation Constellation { get; set; }

    /// <summary>
    /// Gets or sets the star's brightness as visible from Earth.
    /// </summary>
    public double ApparentMagnitude { get; set; }

    public double AbsoluteMagnitude { get; set; }

    public string SpectralType { get; set; }

    public Color Color { get; set; }

    public double RightAscension { get; set; }

    public double Declination { get; set; }

    public double RightAscensionRad { get; set; }

    public double DeclinationRad { get; set; }

    public double X { get; set; }

    public double Y { get; set; }

    public double Z { get; set; }

    public double Luminosity { get; set; }
  }
}
