using StarMap.Cll.Models;
using StarMap.Cll.Models.Cosmos;
using StarMap.Core.Extensions;
using DbEntityStar = StarMap.Dal.Database.Contracts.Star;

namespace StarMap.Dal.Mappers
{
  public static class Stars
  {
    public static Star Map(DbEntityStar source)
    {
      if (source == null)
        return null;

      return new Star()
      {
        Id = source.Id,
        AbsoluteMagnitude = source.AbsoluteMagnitude,
        ApparentMagnitude = source.ApparentMagnitude,
        Designation = GetStarDesignation(source.ProperName, source.BayerName, source.FlamsteedName),
        // probably constellation not needed here also
        ConstellationId = source.ConstellationId,
        Declination = source.Declination,
        DeclinationRad = source.DeclinationRad,
        ParsecDistance = source.ParsecDistance,
        LightYearDistance = GetLightYears(source.ParsecDistance),
        RightAscension = source.RightAscension,
        RightAscensionRad = source.RightAscensionRad,
        X = source.X,
        Y = source.Y,
        Z = source.Z
      };
    }

    public static string GetStarDesignation(string proper, string bayer, string flamsteed)
    {
      return proper ?? (!flamsteed.IsNullOrEmpty()
        ? flamsteed + " " + bayer : bayer);
    }

    // TODO: Move this to some calculator/utils class
    public static double GetLightYears(double parsecs)
    {
      return parsecs * 3.262;
    }
  }
}
