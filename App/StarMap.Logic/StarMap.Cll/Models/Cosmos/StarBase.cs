using StarMap.Core.Extensions;

namespace StarMap.Cll.Models.Cosmos
{
  public abstract class StarBase
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Bayer { get; set; }

    public string Flamsteed { get; set; }

    public double ParsecDistance { get; set; }

    public string Designation => Name ?? (!Flamsteed.IsNullOrEmpty() ? $"{Flamsteed} {Bayer}" : Bayer);

    public double LightYearDistance => ParsecDistance * 3.262;

    // Keep it in case I dont want it here, but in manager or some utils class in CLL.
    //public static string GetStarDesignation(string proper, string bayer, string flamsteed)
    //  => proper ?? (!flamsteed.IsNullOrEmpty() ? $"{flamsteed} {bayer}" : bayer);
    //public static double GetLightYears(double parsecs)
    //  => parsecs* 3.262;
  }
}
