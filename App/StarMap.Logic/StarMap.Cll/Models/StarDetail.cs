using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarMap.Cll.Models
{
  public class StarDetail
  {
    public int Id { get; set; }

    public int? HipparcosId { get; set; }

    public int? HenryDraperId { get; set; }

    public string GlieseId { get; set; }

    /// <summary>
    /// Gets or sets the name of the multi-star system.
    /// </summary>
    public string Base { get; set; }

    public string ProperName { get; set; }

    public string FlamsteedName { get; set; }

    public string BayerName { get; set; }

    public Constellation Constellation { get; set; }

    public double ParsecDistance { get; set; }

    public double LightYearDistance { get; set; }

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
