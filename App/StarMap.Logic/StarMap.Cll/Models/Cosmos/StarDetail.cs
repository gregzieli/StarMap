using Xamarin.Forms;

namespace StarMap.Cll.Models.Cosmos
{
  public class StarDetail : StarBase
  {
    public int? HenryDraperId { get; set; }

    public string GlieseId { get; set; }

    /// <summary>
    /// Gets or sets the name of the multi-star system.
    /// </summary>
    public string Base { get; set; }

    /// <summary>
    /// Gets or sets the star's brightness as visible from Earth.
    /// </summary>
    public double ApparentMagnitude { get; set; }

    public double AbsoluteMagnitude { get; set; }

    public string SpectralType { get; set; }

    public double? ColorIndex { get; set; }

    public Color Color { get; set; }

    public double RightAscension { get; set; }

    public double Declination { get; set; }

    // from https://github.com/astronexus/HYG-Database:
    // Star's luminosity as a multiple of Solar luminosity
    public double Luminosity { get; set; }

    public double? TemperatureKelvin { get; set; }

    public double? TemperatureCelcius => TemperatureKelvin.HasValue ? TemperatureKelvin - 273.15 : null;


    //TODO: override Designation - use real greek letters for Bayer, Constellation in Genetive
  }
}
