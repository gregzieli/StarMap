namespace StarMap.Cll.Models
{
  public class Star
  {
    public int Id { get; set; }

    public string Designation { get; set; }

    public int? ConstellationId { get; set; }

    public double ParsecDistance { get; set; }

    public double LightYearDistance { get; set; }

    /// <summary>
    /// Gets or sets the star's brightness as visible from Earth.
    /// </summary>
    public double ApparentMagnitude { get; set; }

    public double AbsoluteMagnitude { get; set; }

    public double RightAscension { get; set; }

    public double Declination { get; set; }

    public double RightAscensionRad { get; set; }

    public double DeclinationRad { get; set; }

    public double X { get; set; }

    public double Y { get; set; }

    public double Z { get; set; }
  }
}
