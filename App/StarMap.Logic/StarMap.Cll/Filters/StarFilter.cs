namespace StarMap.Cll.Filters
{
  public class StarFilter
  {
    public double DistanceTo { get; set; } = Constants.Filters.DEF_DIST;

    public double MagnitudeTo { get; set; } = Constants.Filters.DEF_MAG;    

    public string DesignationQuery { get; set; }
  }
}
