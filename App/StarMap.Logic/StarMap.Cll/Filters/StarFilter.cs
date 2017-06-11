namespace StarMap.Cll.Filters
{
  public class StarFilter
  {
    public int[] ConstellationsIds { get; set; }

    public double DistanceTo { get; set; } = Constants.Filters.DEF_DIST;

    public double MagnitudeTo { get; set; } = Constants.Filters.DEF_MAG;

    // Just for test property. No connection to the Presentation layer.
    public int? Limit { get; set; }

    public string DesignationQuery { get; set; }
  }
}
